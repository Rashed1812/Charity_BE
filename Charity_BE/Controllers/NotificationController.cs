using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.NotificationDTOs;
using Shared.DTOS.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        public NotificationController(INotificationService notificationService, IEmailService emailService)
        {
            _notificationService = notificationService;
            _emailService = emailService;
        }

        // GET: api/notification
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<NotificationDTO>>>> GetUserNotifications(bool onlyUnread = false)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponse<List<NotificationDTO>>.ErrorResult("User not authenticated", 401));

            var notifications = await _notificationService.GetUserNotificationsAsync(userId, onlyUnread);
            return Ok(ApiResponse<List<NotificationDTO>>.SuccessResult(notifications));
        }

        // POST: api/notification/mark-as-read/{id}
        [HttpPost("mark-as-read/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok(ApiResponse<bool>.SuccessResult(true, "Notification marked as read"));
        }

        // POST: api/notification/send
        [HttpPost("send")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> SendNotification([FromBody] NotificationCreateDTO notificationDto, [FromQuery] string toEmail)
        {
            await _notificationService.AddNotificationAsync(notificationDto);
            if (!string.IsNullOrEmpty(toEmail))
            {
                await _emailService.SendEmailAsync(toEmail, notificationDto.Title, notificationDto.Message);
            }
            return Ok(ApiResponse<bool>.SuccessResult(true, "Notification sent successfully"));
        }
    }
} 