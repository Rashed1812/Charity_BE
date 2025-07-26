using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.ReconcileRequestDTOs;
using Shared.DTOS.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Service;
using Microsoft.AspNetCore.Identity;
using Shared.DTOS.NotificationDTOs;

namespace Charity_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReconcileRequestController : ControllerBase
    {
        private readonly IReconcileRequestService _service;
        private readonly IUserService _userService;
        private readonly IMediationService _meditationService;
        private readonly INotificationService _notificationService;

        public ReconcileRequestController(IReconcileRequestService service,IUserService userService,IMediationService mediationService,INotificationService notificationService)
        {
            _notificationService = notificationService;
            _userService = userService;
            _meditationService = mediationService;
            _service = service;
        }

        // POST: api/reconcilerequest
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> Create(
            [FromQuery] string userId,
            [FromBody] CreateReconcileRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult("User ID is required", 400));
            }

            var created = await _service.CreateAsync(userId, dto);
            var user = await _userService.GetUserByIdAsync(userId);
            var userName = user?.FullName ?? "مستخدم";

            var mediators = await _meditationService.GetAllMediationsAsync();

            foreach (var mediator in mediators)
            {
                var notification = new NotificationCreateDTO
                {
                    UserId = mediator.UserId,
                    Title = "طلب إصلاح ذات بين جديد",
                    Message = $"قام المستخدم {userName} بإرسال طلب إصلاح ذات بين. يُرجى الاطلاع عليه.",
                    Type = NotificationType.General
                };

                await _notificationService.AddNotificationAsync(notification);
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponse<ReconcileRequestDTO>.SuccessResult(created, "Request created successfully"));
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