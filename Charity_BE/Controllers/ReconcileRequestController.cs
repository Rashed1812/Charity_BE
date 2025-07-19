using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.ReconcileRequestDTOs;
using Shared.DTOS.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReconcileRequestController : ControllerBase
    {
        private readonly IReconcileRequestService _service;
        public ReconcileRequestController(IReconcileRequestService service)
        {
            _service = service;
        }

        // POST: api/reconcilerequest
        [HttpPost]
        [Authorize(Roles = "Admin,Mediation")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> Create([FromBody] CreateReconcileRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            var userId = User.FindFirst("sub")?.Value;
            var created = await _service.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<ReconcileRequestDTO>.SuccessResult(created, "Request created successfully"));
        }

        // GET: api/reconcilerequest/user
        [HttpGet("user")]
        [Authorize(Roles = "Admin,Mediation")]
        public async Task<ActionResult<ApiResponse<List<ReconcileRequestDTO>>>> GetUserRequests()
        {
            var userId = User.FindFirst("sub")?.Value;
            var requests = await _service.GetByUserIdAsync(userId);
            return Ok(ApiResponse<List<ReconcileRequestDTO>>.SuccessResult(requests));
        }

        // GET: api/reconcilerequest
        [HttpGet]
        [Authorize(Roles = "Admin,Mediation")]
        public async Task<ActionResult<ApiResponse<List<ReconcileRequestDTO>>>> GetAll()
        {
            var requests = await _service.GetAllAsync();
            return Ok(ApiResponse<List<ReconcileRequestDTO>>.SuccessResult(requests));
        }

        // GET: api/reconcilerequest/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Mediation")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> GetById(int id)
        {
            var request = await _service.GetByIdAsync(id);
            if (request == null)
                return NotFound(ApiResponse<ReconcileRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));
            return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request));
        }

        // DELETE: api/reconcilerequest/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Mediation")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var userId = User.FindFirst("sub")?.Value;
            var result = await _service.DeleteAsync(id, userId);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResult($"Request with ID {id} not found or not allowed", 404));
            return Ok(ApiResponse<bool>.SuccessResult(true, "Request deleted successfully"));
        }
    }
} 