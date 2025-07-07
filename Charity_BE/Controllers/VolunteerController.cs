using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.VolunteerDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using DAL.Data.Models;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerService _volunteerService;

        public VolunteerController(IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
        }

        // GET: api/volunteer
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<VolunteerApplicationDTO>>>> GetAllApplications()
        {
            try
            {
                var applications = await _volunteerService.GetAllApplicationsAsync();
                return Ok(ApiResponse<List<VolunteerApplicationDTO>>.SuccessResult(applications));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<VolunteerApplicationDTO>>.ErrorResult("Failed to retrieve applications", 500));
            }
        }

        // GET: api/volunteer/user
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<VolunteerApplicationDTO>>> GetUserApplication()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<VolunteerApplicationDTO>.ErrorResult("User not authenticated", 401));

                var application = await _volunteerService.GetUserApplicationAsync(userId);
                if (application == null)
                    return NotFound(ApiResponse<VolunteerApplicationDTO>.ErrorResult("No application found for this user", 404));

                return Ok(ApiResponse<VolunteerApplicationDTO>.SuccessResult(application));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VolunteerApplicationDTO>.ErrorResult("Failed to retrieve application", 500));
            }
        }

        // GET: api/volunteer/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<VolunteerApplicationDTO>>> GetApplicationById(int id)
        {
            try
            {
                var application = await _volunteerService.GetApplicationByIdAsync(id);
                if (application == null)
                    return NotFound(ApiResponse<VolunteerApplicationDTO>.ErrorResult($"Application with ID {id} not found", 404));

                return Ok(ApiResponse<VolunteerApplicationDTO>.SuccessResult(application));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VolunteerApplicationDTO>.ErrorResult("Failed to retrieve application", 500));
            }
        }

        // POST: api/volunteer
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<VolunteerApplicationDTO>>> CreateApplication([FromBody] CreateVolunteerApplicationDTO createApplicationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<VolunteerApplicationDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<VolunteerApplicationDTO>.ErrorResult("User not authenticated", 401));

                var application = await _volunteerService.CreateApplicationAsync(userId, createApplicationDto);
                return CreatedAtAction(nameof(GetApplicationById), new { id = application.Id }, 
                    ApiResponse<VolunteerApplicationDTO>.SuccessResult(application, "Application submitted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VolunteerApplicationDTO>.ErrorResult("Failed to submit application", 500));
            }
        }

        // PUT: api/volunteer/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<VolunteerApplicationDTO>>> UpdateApplication(int id, [FromBody] UpdateVolunteerApplicationDTO updateApplicationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<VolunteerApplicationDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<VolunteerApplicationDTO>.ErrorResult("User not authenticated", 401));

                var application = await _volunteerService.UpdateApplicationAsync(id, userId, updateApplicationDto);
                if (application == null)
                    return NotFound(ApiResponse<VolunteerApplicationDTO>.ErrorResult($"Application with ID {id} not found", 404));

                return Ok(ApiResponse<VolunteerApplicationDTO>.SuccessResult(application, "Application updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VolunteerApplicationDTO>.ErrorResult("Failed to update application", 500));
            }
        }

        // DELETE: api/volunteer/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteApplication(int id)
        {
            try
            {
                var result = await _volunteerService.DeleteApplicationAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Application with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Application deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete application", 500));
            }
        }

        // PUT: api/volunteer/{id}/review
        [HttpPut("{id}/review")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<VolunteerApplicationDTO>>> ReviewApplication(int id, [FromBody] ReviewVolunteerApplicationDTO reviewDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<VolunteerApplicationDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var adminId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(ApiResponse<VolunteerApplicationDTO>.ErrorResult("Admin not authenticated", 401));

                var application = await _volunteerService.ReviewApplicationAsync(id, adminId, reviewDto);
                if (application == null)
                    return NotFound(ApiResponse<VolunteerApplicationDTO>.ErrorResult($"Application with ID {id} not found", 404));

                return Ok(ApiResponse<VolunteerApplicationDTO>.SuccessResult(application, "Application reviewed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VolunteerApplicationDTO>.ErrorResult("Failed to review application", 500));
            }
        }

        // GET: api/volunteer/status/{status}
        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<VolunteerApplicationDTO>>>> GetApplicationsByStatus(VolunteerStatus status)
        {
            try
            {
                var applications = await _volunteerService.GetApplicationsByStatusAsync(status);
                return Ok(ApiResponse<List<VolunteerApplicationDTO>>.SuccessResult(applications));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<VolunteerApplicationDTO>>.ErrorResult("Failed to retrieve applications", 500));
            }
        }

        // GET: api/volunteer/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetVolunteerStatistics()
        {
            try
            {
                var statistics = await _volunteerService.GetVolunteerStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }
    }
} 