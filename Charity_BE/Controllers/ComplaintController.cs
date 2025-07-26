using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.ComplaintDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using Microsoft.AspNetCore.Identity;
using Shared.DTOS.NotificationDTOs;
using DAL.Data.Models;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;
        IAdminService _adminService;
        INotificationService _notificationService;
        IUserService _userService;


        public ComplaintController(IComplaintService complaintService, IAdminService adminService, INotificationService notificationService,IUserService userService)
        {
            _userService = userService;
            _complaintService = complaintService;
            _adminService = adminService;
            _notificationService = notificationService;
        }

        // GET: api/complaint
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<ComplaintDTO>>>> GetAllComplaints()
        {
            try
            {
                var complaints = await _complaintService.GetAllComplaintsAsync();
                return Ok(ApiResponse<List<ComplaintDTO>>.SuccessResult(complaints));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ComplaintDTO>>.ErrorResult("Failed to retrieve complaints", 500));
            }
        }

        // GET: api/complaint/user
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ComplaintDTO>>>> GetUserComplaints()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<List<ComplaintDTO>>.ErrorResult("User not authenticated", 401));

                var complaints = await _complaintService.GetUserComplaintsAsync(userId);
                return Ok(ApiResponse<List<ComplaintDTO>>.SuccessResult(complaints));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ComplaintDTO>>.ErrorResult("Failed to retrieve complaints", 500));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ComplaintDTO>>> Create(
            [FromQuery] string userId,
            [FromBody] CreateComplaintDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ComplaintDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<ComplaintDTO>.ErrorResult("User ID is required", 400));

            var createdComplaint = await _complaintService.CreateComplaintAsync(userId, dto);

            // جلب اسم المستخدم
            var user = await _userService.GetUserByIdAsync(userId);
            var userName = user.FullName;

            // إرسال إشعار لكل الأدمنز
            var admins = await _adminService.GetAllAdminsAsync();
            foreach (var admin in admins)
            {
                var notification = new NotificationCreateDTO
                {
                    UserId = admin.UserId,
                    Title = "شكوى جديدة",
                    Message = $"قام المستخدم {userName} بتقديم شكوى جديدة. يرجى مراجعتها.",
                    Type = NotificationType.General
                };
                await _notificationService.AddNotificationAsync(notification);
            }

            return Ok(ApiResponse<ComplaintDTO>.SuccessResult(createdComplaint, "Complaint created successfully"));
        }


        // PUT: api/complaint/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ComplaintDTO>>> UpdateComplaint(int id, [FromBody] UpdateComplaintDTO updateComplaintDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ComplaintDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var complaint = await _complaintService.UpdateComplaintAsync(id, updateComplaintDto);
                if (complaint == null)
                    return NotFound(ApiResponse<ComplaintDTO>.ErrorResult($"Complaint with ID {id} not found", 404));

                return Ok(ApiResponse<ComplaintDTO>.SuccessResult(complaint, "Complaint updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ComplaintDTO>.ErrorResult("Failed to update complaint", 500));
            }
        }

        // DELETE: api/complaint/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteComplaint(int id)
        {
            try
            {
                var result = await _complaintService.DeleteComplaintAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Complaint with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Complaint deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete complaint", 500));
            }
        }

        // PUT: api/complaint/{id}/status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ComplaintDTO>>> UpdateComplaintStatus(int id, [FromBody] ComplaintStatus status)
        {
            try
            {
                var complaint = await _complaintService.UpdateComplaintStatusAsync(id, status);
                if (complaint == null)
                    return NotFound(ApiResponse<ComplaintDTO>.ErrorResult($"Complaint with ID {id} not found", 404));

                return Ok(ApiResponse<ComplaintDTO>.SuccessResult(complaint, "Complaint status updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ComplaintDTO>.ErrorResult("Failed to update complaint status", 500));
            }
        }

        // GET: api/complaint/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetComplaintStatistics()
        {
            try
            {
                var statistics = await _complaintService.GetComplaintStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }
    }
} 