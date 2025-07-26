using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.UserDTO;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using BLL.Service;
using Shared.DTOS.NotificationDTOs;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public UserController(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UserDTO>>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(ApiResponse<List<UserDTO>>.SuccessResult(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<UserDTO>>.ErrorResult("Failed to retrieve users", 500));
            }
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDTO>>> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(ApiResponse<UserDTO>.ErrorResult($"User with ID {id} not found", 404));

                return Ok(ApiResponse<UserDTO>.SuccessResult(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDTO>.ErrorResult("Failed to retrieve user", 500));
            }
        }

        // GET: api/user/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<ApiResponse<UserDTO>>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                    return NotFound(ApiResponse<UserDTO>.ErrorResult($"User with email {email} not found", 404));

                return Ok(ApiResponse<UserDTO>.SuccessResult(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDTO>.ErrorResult("Failed to retrieve user", 500));
            }
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserDTO>>> UpdateUser(string id, [FromBody] UpdateUserDTO updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<UserDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDto);
                if (user == null)
                    return NotFound(ApiResponse<UserDTO>.ErrorResult($"User with ID {id} not found", 404));

                return Ok(ApiResponse<UserDTO>.SuccessResult(user, "User updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<UserDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDTO>.ErrorResult("Failed to update user", 500));
            }
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(string id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"User with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "User deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete user", 500));
            }
        }

        // POST: api/user/{id}/activate
        [HttpPost("{id}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateUser(string id)
        {
            try
            {
                var result = await _userService.ActivateUserAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"User with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "User activated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to activate user", 500));
            }
        }

        // POST: api/user/{id}/deactivate
        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult<ApiResponse<bool>>> DeactivateUser(string id)
        {
            try
            {
                var result = await _userService.DeactivateUserAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"User with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "User deactivated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to deactivate user", 500));
            }
        }

        // GET: api/user/role/{role}
        [HttpGet("role/{role}")]
        public async Task<ActionResult<ApiResponse<List<UserDTO>>>> GetUsersByRole(string role)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(role);
                return Ok(ApiResponse<List<UserDTO>>.SuccessResult(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<UserDTO>>.ErrorResult("Failed to retrieve users", 500));
            }
        }

        // GET: api/user/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<ApiResponse<object>>> GetUserStatistics()
        {
            try
            {
                var statistics = await _userService.GetUserStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }
        [HttpGet("notifications")]
        public async Task<ActionResult<ApiResponse<List<NotificationDTO>>>> GetMyNotifications(
            [FromQuery] string userId,
            [FromQuery] bool onlyUnread = false)
        {
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponse<List<NotificationDTO>>.ErrorResult("User ID is missing", 401));

            var notifications = await _notificationService.GetUserNotificationsAsync(userId, onlyUnread);
            return Ok(ApiResponse<List<NotificationDTO>>.SuccessResult(notifications));
        }
        [HttpPatch("notifications/{notificationId}/read")]
        public async Task<ActionResult<ApiResponse<string>>> MarkNotificationAsRead(int notificationId)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(notificationId);
                return Ok(ApiResponse<string>.SuccessResult("Notification marked as read successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<string>.ErrorResult("Notification not found", 404));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("An error occurred while marking the notification as read", 500));
            }
        }
    }
}
