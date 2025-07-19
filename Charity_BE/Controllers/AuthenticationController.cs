using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AuthDTO;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/authentication/login
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var result = await _authService.LoginAsync(loginDto);
                if (!result.Success)
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult(result.Message, 400));

                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AuthResponseDTO>.ErrorResult("Failed to login", 500));
            }
        }

        // POST: api/authentication/register
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                if (!result.Success)
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult(result.Message, 400));

                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AuthResponseDTO>.ErrorResult(ex.Message, 500));
            }
        }

        // POST: api/authentication/register-admin
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> RegisterAdmin([FromBody] RegisterAdminDTO registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var result = await _authService.RegisterAdminAsync(registerDto);
                if (!result.Success)
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult(result.Message, 400));

                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AuthResponseDTO>.ErrorResult("Failed to register admin", 500));
            }
        }

        // POST: api/authentication/register-advisor
        [HttpPost("register-advisor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> RegisterAdvisor([FromBody] RegisterAdvisorDTO registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var result = await _authService.RegisterAdvisorAsync(registerDto);
                if (!result.Success)
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult(result.Message, 400));

                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AuthResponseDTO>.ErrorResult("Failed to register advisor", 500));
            }
        }

        // GET: api/authentication/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CurrentUserDTO>>> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<CurrentUserDTO>.ErrorResult("User not authenticated", 401));

                var user = await _authService.GetCurrentUserAsync(userId);
                if (user == null)
                    return NotFound(ApiResponse<CurrentUserDTO>.ErrorResult("User not found", 404));

                return Ok(ApiResponse<CurrentUserDTO>.SuccessResult(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CurrentUserDTO>.ErrorResult("Failed to get current user", 500));
            }
        }
    }
}
