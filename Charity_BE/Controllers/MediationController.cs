using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.MediationDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediationController : ControllerBase
    {
        private readonly IMediationService _mediationService;

        public MediationController(IMediationService mediationService)
        {
            _mediationService = mediationService;
        }

        // GET: api/mediation
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<MediationDTO>>>> GetAllMediations()
        {
            try
            {
                var mediations = await _mediationService.GetAllMediationsAsync();
                return Ok(ApiResponse<List<MediationDTO>>.SuccessResult(mediations));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<MediationDTO>>.ErrorResult(ex.Message, 500));
            }
        }

        // GET: api/mediation/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MediationDTO>>> GetMediationById(int id)
        {
            try
            {
                var mediation = await _mediationService.GetMediationByIdAsync(id);
                if (mediation == null)
                    return NotFound(ApiResponse<MediationDTO>.ErrorResult($"Mediation with ID {id} not found", 404));
                return Ok(ApiResponse<MediationDTO>.SuccessResult(mediation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<MediationDTO>.ErrorResult(ex.Message, 500));
            }
        }

        // POST: api/mediation
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<MediationDTO>>> CreateMediation([FromForm] CreateMediationDTO createMediationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<MediationDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            try
            {
                var mediation = await _mediationService.CreateMediationAsync(createMediationDto);
                return CreatedAtAction(nameof(GetMediationById), new { id = mediation.Id },
                    ApiResponse<MediationDTO>.SuccessResult(mediation, "Mediation created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<MediationDTO>.ErrorResult(ex.Message, 500));
            }
        }

        // PUT: api/mediation/{id}
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Mediation")]
        public async Task<ActionResult<ApiResponse<MediationDTO>>> UpdateMediation(int id, [FromForm] UpdateMediationDTO updateMediationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<MediationDTO>.ErrorResult("Invalid input data", 400));
            try
            {
                var mediation = await _mediationService.UpdateMediationAsync(id, updateMediationDto);
                if (mediation == null)
                    return NotFound(ApiResponse<MediationDTO>.ErrorResult($"Mediation with ID {id} not found", 404));
                return Ok(ApiResponse<MediationDTO>.SuccessResult(mediation, "Mediation updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<MediationDTO>.ErrorResult(ex.Message, 500));
            }
        }

        // DELETE: api/mediation/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMediation(int id)
        {
            try
            {
                var result = await _mediationService.DeleteMediationAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Mediation with ID {id} not found", 404));
                return Ok(ApiResponse<bool>.SuccessResult(true, "Mediation deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult(ex.Message, 500));
            }
        }
    }
} 