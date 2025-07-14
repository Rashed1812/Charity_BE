using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AdminDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/admin
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<AdminDTO>>>> GetAllAdmins()
        {
            try
            {
                var admins = await _adminService.GetAllAdminsAsync();
                return Ok(ApiResponse<List<AdminDTO>>.SuccessResult(admins));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdminDTO>>.ErrorResult("Failed to retrieve admins", 500));
            }
        }

        // GET: api/admin/test (Temporary endpoint for testing without authentication)
        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<AdminDTO>>>> GetAllAdminsTest()
        {
            try
            {
                var admins = await _adminService.GetAllAdminsAsync();
                return Ok(ApiResponse<List<AdminDTO>>.SuccessResult(admins));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdminDTO>>.ErrorResult("Failed to retrieve admins", 500));
            }
        }

        // GET: api/admin/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AdminDTO>>> GetAdminById(int id)
        {
            try
            {
                var admin = await _adminService.GetAdminByIdAsync(id);
                if (admin == null)
                    return NotFound(ApiResponse<AdminDTO>.ErrorResult($"Admin with ID {id} not found", 404));

                return Ok(ApiResponse<AdminDTO>.SuccessResult(admin));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdminDTO>.ErrorResult("Failed to retrieve admin", 500));
            }
        }

        // GET: api/admin/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<AdminDTO>>> GetAdminByUserId(string userId)
        {
            try
            {
                var admin = await _adminService.GetAdminByUserIdAsync(userId);
                if (admin == null)
                    return NotFound(ApiResponse<AdminDTO>.ErrorResult($"Admin with User ID {userId} not found", 404));

                return Ok(ApiResponse<AdminDTO>.SuccessResult(admin));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdminDTO>.ErrorResult("Failed to retrieve admin", 500));
            }
        }

        // POST: api/admin
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AdminDTO>>> CreateAdmin([FromBody] CreateAdminDTO createAdminDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdminDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var admin = await _adminService.CreateAdminAsync(createAdminDto);
                return CreatedAtAction(nameof(GetAdminById), new { id = admin.Id }, 
                    ApiResponse<AdminDTO>.SuccessResult(admin, "Admin created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdminDTO>.ErrorResult("Failed to create admin", 500));
            }
        }

        // PUT: api/admin/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AdminDTO>>> UpdateAdmin(int id, [FromBody] UpdateAdminDTO updateAdminDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdminDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var admin = await _adminService.UpdateAdminAsync(id, updateAdminDto);
                if (admin == null)
                    return NotFound(ApiResponse<AdminDTO>.ErrorResult($"Admin with ID {id} not found", 404));

                return Ok(ApiResponse<AdminDTO>.SuccessResult(admin, "Admin updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdminDTO>.ErrorResult("Failed to update admin", 500));
            }
        }

        // DELETE: api/admin/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAdmin(int id)
        {
            try
            {
                var result = await _adminService.DeleteAdminAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Admin with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Admin deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete admin", 500));
            }
        }

        // GET: api/admin/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<ApiResponse<object>>> GetSystemStatistics()
        {
            try
            {
                var statistics = await _adminService.GetSystemStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve system statistics", 500));
            }
        }

        // GET: api/admin/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<ApiResponse<object>>> GetDashboardData()
        {
            try
            {
                var dashboardData = await _adminService.GetDashboardDataAsync();
                return Ok(ApiResponse<object>.SuccessResult(dashboardData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve dashboard data", 500));
            }
        }

        // POST: api/admin/user/{userId}/approve
        [HttpPost("user/{userId}/approve")]
        public async Task<ActionResult<ApiResponse<bool>>> ApproveUser(string userId)
        {
            try
            {
                var result = await _adminService.ApproveUserAsync(userId);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"User with ID {userId} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "User approved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to approve user", 500));
            }
        }

        // POST: api/admin/user/{userId}/reject
        [HttpPost("user/{userId}/reject")]
        public async Task<ActionResult<ApiResponse<bool>>> RejectUser(string userId, [FromBody] string reason)
        {
            try
            {
                var result = await _adminService.RejectUserAsync(userId, reason);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"User with ID {userId} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "User rejected successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to reject user", 500));
            }
        }

        // POST: api/admin/user/{userId}/suspend
        [HttpPost("user/{userId}/suspend")]
        public async Task<ActionResult<ApiResponse<bool>>> SuspendUser(string userId, [FromBody] SuspendUserDTO suspendDto)
        {
            try
            {
                var result = await _adminService.SuspendUserAsync(userId, suspendDto.Reason, suspendDto.Until);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"User with ID {userId} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "User suspended successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to suspend user", 500));
            }
        }

        // POST: api/admin/user/{userId}/activate
        [HttpPost("user/{userId}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateUser(string userId)
        {
            try
            {
                var result = await _adminService.ActivateUserAsync(userId);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"User with ID {userId} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "User activated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to activate user", 500));
            }
        }

        // GET: api/admin/logs
        [HttpGet("logs")]
        public async Task<ActionResult<ApiResponse<List<object>>>> GetSystemLogs([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string logLevel)
        {
            try
            {
                var logs = await _adminService.GetSystemLogsAsync(fromDate, toDate, logLevel);
                return Ok(ApiResponse<List<object>>.SuccessResult(logs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<object>>.ErrorResult("Failed to retrieve system logs", 500));
            }
        }

        // POST: api/admin/notification
        [HttpPost("notification")]
        public async Task<ActionResult<ApiResponse<bool>>> SendSystemNotification([FromBody] SendNotificationDTO notificationDto)
        {
            try
            {
                var result = await _adminService.SendSystemNotificationAsync(notificationDto.Title, notificationDto.Message, notificationDto.UserIds);
                return Ok(ApiResponse<bool>.SuccessResult(result, "Notification sent successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to send notification", 500));
            }
        }
    }

    public class SuspendUserDTO
    {
        public string Reason { get; set; }
        public DateTime? Until { get; set; }
    }

    public class SendNotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public List<string> UserIds { get; set; }
    }
} 