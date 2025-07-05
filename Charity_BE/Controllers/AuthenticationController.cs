using BLL.DTOS.AuthDTO;
using BLL.ServiceAbstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);
            if (response == null)
            {
                return BadRequest("Registration failed");
            }
            return Ok(response);
        }
        [HttpPost("register/advisor")]
        public async Task<IActionResult> RegisterAdvisor([FromBody] RegisterAdvisorDTO registerAdvisorDto)
        {
            var response = await _authService.RegisterAdvisorAsync(registerAdvisorDto);
            if (response == null)
            {
                return BadRequest("Advisor registration failed");
            }
            return Ok(response);
        }
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDTO registerAdminDto)
        {
            var response = await _authService.RegisterAdminAsync(registerAdminDto);
            if (response == null)
            {
                return BadRequest("Admin registration failed");
            }
            return Ok(response);
        }
        [HttpGet("MyProfile")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var userDto = await _authService.GetCurrentUserAsync(email);
            return Ok(userDto);
        }
    }
}
