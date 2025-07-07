using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.ConsultationDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;

        public ConsultationController(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        // GET: api/consultation
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ConsultationDTO>>>> GetAllConsultations()
        {
            try
            {
                var consultations = await _consultationService.GetAllConsultationsAsync();
                return Ok(ApiResponse<IEnumerable<ConsultationDTO>>.SuccessResult(consultations));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ConsultationDTO>>.ErrorResult("Failed to retrieve consultations", 500));
            }
        }

        // GET: api/consultation/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<ConsultationDTO>>>> GetActiveConsultations()
        {
            try
            {
                var consultations = await _consultationService.GetActiveConsultationsAsync();
                return Ok(ApiResponse<List<ConsultationDTO>>.SuccessResult(consultations));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ConsultationDTO>>.ErrorResult("Failed to retrieve active consultations", 500));
            }
        }

        // GET: api/consultation/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ConsultationDTO>>> GetConsultationById(int id)
        {
            try
            {
                var consultation = await _consultationService.GetConsultationByIdAsync(id);
                if (consultation == null)
                    return NotFound(ApiResponse<ConsultationDTO>.ErrorResult($"Consultation with ID {id} not found", 404));

                return Ok(ApiResponse<ConsultationDTO>.SuccessResult(consultation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ConsultationDTO>.ErrorResult("Failed to retrieve consultation", 500));
            }
        }

        // POST: api/consultation
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ConsultationDTO>>> CreateConsultation([FromBody] CreateConsultationDTO createConsultationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ConsultationDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var consultation = await _consultationService.CreateConsultationAsync(createConsultationDto);
                return CreatedAtAction(nameof(GetConsultationById), new { id = consultation.Id }, 
                    ApiResponse<ConsultationDTO>.SuccessResult(consultation, "Consultation created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ConsultationDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ConsultationDTO>.ErrorResult("Failed to create consultation", 500));
            }
        }

        // PUT: api/consultation/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ConsultationDTO>>> UpdateConsultation(int id, [FromBody] UpdateConsultationDTO updateConsultationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ConsultationDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var consultation = await _consultationService.UpdateConsultationAsync(id, updateConsultationDto);
                if (consultation == null)
                    return NotFound(ApiResponse<ConsultationDTO>.ErrorResult($"Consultation with ID {id} not found", 404));

                return Ok(ApiResponse<ConsultationDTO>.SuccessResult(consultation, "Consultation updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ConsultationDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ConsultationDTO>.ErrorResult("Failed to update consultation", 500));
            }
        }

        // DELETE: api/consultation/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteConsultation(int id)
        {
            try
            {
                var result = await _consultationService.DeleteConsultationAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Consultation with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Consultation deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete consultation", 500));
            }
        }

        // GET: api/consultation/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetConsultationStatistics()
        {
            try
            {
                var statistics = await _consultationService.GetConsultationStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }
    }
}
