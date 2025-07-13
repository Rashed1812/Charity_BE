using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AdvisorDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorController : ControllerBase
    {
        private readonly IAdvisorService _advisorService;

        public AdvisorController(IAdvisorService advisorService)
        {
            _advisorService = advisorService;
        }

        // GET: api/advisor
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<AdvisorDTO>>>> GetAllAdvisors()
        {
            try
            {
                var advisors = await _advisorService.GetAllAdvisorsAsync();
                return Ok(ApiResponse<List<AdvisorDTO>>.SuccessResult(advisors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorDTO>>.ErrorResult("Failed to retrieve advisors", 500));
            }

        }
        [HttpGet("with-availability")]
        public async Task<ActionResult<ApiResponse<List<AdvisorDTO>>>> GetAllAdvisorsWithRelatedData()
        {
            try
            {
                var advisors = await _advisorService.GetAllAdvisorsWithAvailabilityAsync();
                return Ok(ApiResponse<List<AdvisorDTO>>.SuccessResult(advisors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorDTO>>.ErrorResult("Failed to retrieve advisors", 500));
            }
        }
        // GET: api/advisor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AdvisorDTO>>> GetAdvisorById(int id)
        {
            try
            {
                var advisor = await _advisorService.GetAdvisorByIdAsync(id);
                if (advisor == null)
                    return NotFound(ApiResponse<AdvisorDTO>.ErrorResult($"Advisor with ID {id} not found", 404));

                return Ok(ApiResponse<AdvisorDTO>.SuccessResult(advisor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdvisorDTO>.ErrorResult("Failed to retrieve advisor", 500));
            }
        }

        // GET: api/advisor/consultation/{consultationId}
        [HttpGet("consultation/{consultationId}")]
        public async Task<ActionResult<ApiResponse<List<AdvisorDTO>>>> GetAdvisorsByConsultation(int consultationId)
        {
            try
            {
                var advisors = await _advisorService.GetAdvisorsByConsultationAsync(consultationId);
                return Ok(ApiResponse<List<AdvisorDTO>>.SuccessResult(advisors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorDTO>>.ErrorResult("Failed to retrieve advisors", 500));
            }
        }

        // POST: api/advisor
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AdvisorDTO>>> CreateAdvisor([FromForm] CreateAdvisorDTO createAdvisorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdvisorDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var advisor = await _advisorService.CreateAdvisorAsync(createAdvisorDto);
                return CreatedAtAction(nameof(GetAdvisorById), new { id = advisor.Id }, 
                    ApiResponse<AdvisorDTO>.SuccessResult(advisor, "Advisor created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdvisorDTO>.ErrorResult("Failed to create advisor", 500));
            }
        }

        // PUT: api/advisor/{id}
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Advisor")]
        public async Task<ActionResult<ApiResponse<AdvisorDTO>>> UpdateAdvisor(int id, [FromForm] UpdateAdvisorDTO updateAdvisorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdvisorDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var advisor = await _advisorService.UpdateAdvisorAsync(id, updateAdvisorDto);
                if (advisor == null)
                    return NotFound(ApiResponse<AdvisorDTO>.ErrorResult($"Advisor with ID {id} not found", 404));

                return Ok(ApiResponse<AdvisorDTO>.SuccessResult(advisor, "Advisor updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdvisorDTO>.ErrorResult("Failed to update advisor", 500));
            }
        }

        // DELETE: api/advisor/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAdvisor(int id)
        {
            try
            {
                var result = await _advisorService.DeleteAdvisorAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Advisor with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Advisor deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete advisor", 500));
            }
        }

        // GET: api/advisor/{id}/availability
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<ApiResponse<List<AdvisorAvailabilityDTO>>>> GetAdvisorAvailability(int id)
        {
            try
            {
                var availabilities = await _advisorService.GetAdvisorAvailabilityAsync(id);
                return Ok(ApiResponse<List<AdvisorAvailabilityDTO>>.SuccessResult(availabilities));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorAvailabilityDTO>>.ErrorResult("Failed to retrieve availability", 500));
            }
        }

        // POST: api/advisor/availability
        [HttpPost("availability")]
        [Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<AdvisorAvailabilityDTO>>> CreateAvailability([FromBody] CreateAvailabilityDTO createAvailabilityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdvisorAvailabilityDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var availability = await _advisorService.CreateAvailabilityAsync(createAvailabilityDto);
                return CreatedAtAction(nameof(GetAdvisorAvailability), new { id = availability.AdvisorId }, 
                    ApiResponse<AdvisorAvailabilityDTO>.SuccessResult(availability, "Availability created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdvisorAvailabilityDTO>.ErrorResult("Failed to create availability", 500));
            }
        }

        // POST: api/advisor/bulk-availability
        [HttpPost("bulk-availability")]
        public async Task<ActionResult<ApiResponse<List<AdvisorAvailabilityDTO>>>> CreateBulkAvailability([FromBody] BulkAvailabilityDTO bulkAvailabilityDto)
        {
            try
            {
                var availabilities = new List<AdvisorAvailabilityDTO>();
                foreach (var availabilityDto in bulkAvailabilityDto.Availabilities)
                {
                    var created = await _advisorService.CreateAvailabilityAsync(availabilityDto);
                    availabilities.Add(created);
                }
                return Ok(ApiResponse<List<AdvisorAvailabilityDTO>>.SuccessResult(availabilities, "Bulk availability created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorAvailabilityDTO>>.ErrorResult("Failed to create bulk availability", 500));
            }
        }

        // DELETE: api/advisor/availability/{id}
        [HttpDelete("availability/{id}")]
        [Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAvailability(int id)
        {
            try
            {
                var result = await _advisorService.DeleteAvailabilityAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Availability with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Availability deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete availability", 500));
            }
        }

        // GET: api/advisor/{id}/requests
        [HttpGet("{id}/requests")]
        [Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<List<AdvisorRequestDTO>>>> GetAdvisorRequests(int id)
        {
            try
            {
                var requests = await _advisorService.GetAdvisorRequestsAsync(id);
                return Ok(ApiResponse<List<AdvisorRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // PUT: api/advisor/requests/{requestId}/status
        [HttpPut("requests/{requestId}/status")]
        [Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<AdvisorRequestDTO>>> UpdateRequestStatus(int requestId, [FromBody] ConsultationStatus status)
        {
            try
            {
                var request = await _advisorService.UpdateRequestStatusAsync(requestId, status);
                if (request == null)
                    return NotFound(ApiResponse<AdvisorRequestDTO>.ErrorResult($"Request with ID {requestId} not found", 404));

                return Ok(ApiResponse<AdvisorRequestDTO>.SuccessResult(request, "Request status updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdvisorRequestDTO>.ErrorResult("Failed to update request status", 500));
            }
        }
        // GET: api/advisor/{advisorId}/available-slots
        [HttpGet("{advisorId}/available-slots")]
        public async Task<ActionResult<ApiResponse<List<AdvisorAvailabilityDTO>>>> GetAvailableSlots(int advisorId, DateTime date)
        {
            try
            {
                var availabilities = await _advisorService.GetAvailableSlotsAsync(advisorId, date);
                return Ok(ApiResponse<List<AdvisorAvailabilityDTO>>.SuccessResult(availabilities));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorAvailabilityDTO>>.ErrorResult("Failed to retrieve available slots", 500));
            }
        }

        // GET: api/advisor/{advisorId}/available-slots-by-type
        [HttpGet("{advisorId}/available-slots-by-type")]
        public async Task<ActionResult<ApiResponse<List<AdvisorAvailabilityDTO>>>> GetAvailableSlotsByType(int advisorId, DateTime date, ConsultationType consultationType)
        {
            try
            {
                var availabilities = await _advisorService.GetAvailableSlotsByTypeAsync(advisorId, date, consultationType);
                return Ok(ApiResponse<List<AdvisorAvailabilityDTO>>.SuccessResult(availabilities));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdvisorAvailabilityDTO>>.ErrorResult("Failed to retrieve available slots", 500));
            }
        }
    }
} 