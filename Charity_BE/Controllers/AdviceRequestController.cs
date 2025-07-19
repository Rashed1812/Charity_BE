using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AdviceRequestDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using Shared.DTOS.AdvisorDTOs;
using System.Security.Claims;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdviceRequestController : ControllerBase
    {
        private readonly IAdviceRequestService _adviceRequestService;

        public AdviceRequestController(IAdviceRequestService adviceRequestService)
        {
            _adviceRequestService = adviceRequestService;
        }

        // GET: api/advicerequest
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetAllRequests()
        {
            try
            {
                var requests = await _adviceRequestService.GetAllRequestsAsync();
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/user
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetUserRequests()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<List<AdviceRequestDTO>>.ErrorResult("User not authenticated", 401));

                var requests = await _adviceRequestService.GetUserRequestsAsync(userId);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/{id}
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> GetRequestById(int id)
        {
            try
            {
                var request = await _adviceRequestService.GetRequestByIdAsync(id);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to retrieve request", 500));
            }
        }

        // POST: api/advicerequest
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> CreateRequest([FromQuery] string userId,[FromBody] CreateAdviceRequestDTO createRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("User ID is required", 400));

                var request = await _adviceRequestService.CreateRequestAsync(userId, createRequestDto);
                return CreatedAtAction(nameof(GetRequestById), new { id = request.Id },
                    ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult(ex.Message, 500));
            }
        }

        // PUT: api/advicerequest/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> UpdateRequest(int id, [FromBody] UpdateAdviceRequestDTO updateRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<AdviceRequestDTO>.ErrorResult("User not authenticated", 401));

                var request = await _adviceRequestService.UpdateRequestAsync(id, userId, updateRequestDto);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to update request", 500));
            }
        }

        // DELETE: api/advicerequest/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> CancelRequest(int id)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<bool>.ErrorResult("User not authenticated", 401));

                var result = await _adviceRequestService.CancelRequestAsync(id, userId);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Request cancelled successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to cancel request", 500));
            }
        }

        // PUT: api/advicerequest/{id}/confirm
        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> ConfirmRequest(int id)
        {
            try
            {
                var advisorId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(advisorId))
                    return Unauthorized(ApiResponse<AdviceRequestDTO>.ErrorResult("Advisor not authenticated", 401));

                var request = await _adviceRequestService.ConfirmRequestAsync(id, advisorId);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request confirmed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to confirm request", 500));
            }
        }

        // PUT: api/advicerequest/{id}/complete
        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> CompleteRequest(int id, [FromBody] CompleteRequestDTO completeRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var advisorId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(advisorId))
                    return Unauthorized(ApiResponse<AdviceRequestDTO>.ErrorResult("Advisor not authenticated", 401));

                var request = await _adviceRequestService.CompleteRequestAsync(id, advisorId, completeRequestDto);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request completed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to complete request", 500));
            }
        }

        // GET: api/advicerequest/status/{status}
        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetRequestsByStatus(ConsultationStatus status)
        {
            try
            {
                var requests = await _adviceRequestService.GetRequestsByStatusAsync(status);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/advisor/{advisorId}
        [HttpGet("advisor/{advisorId}")]
        [Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetRequestsByAdvisor(int advisorId)
        {
            try
            {
                var requests = await _adviceRequestService.GetRequestsByAdvisorAsync(advisorId);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/consultation/{consultationId}
        [HttpGet("consultation/{consultationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetRequestsByConsultation(int consultationId)
        {
            try
            {
                var requests = await _adviceRequestService.GetRequestsByConsultationAsync(consultationId);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetRequestStatistics()
        {
            try
            {
                var statistics = await _adviceRequestService.GetRequestStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }
    }
} 