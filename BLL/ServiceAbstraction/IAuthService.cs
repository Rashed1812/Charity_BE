using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.AuthDTO;

namespace BLL.ServiceAbstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<AuthResponseDTO> RegisterAdminAsync(RegisterAdminDTO registerDto);
        Task<AuthResponseDTO> RegisterAdvisorAsync(RegisterAdvisorDTO registerDto);
        Task<CurrentUserDTO> GetCurrentUserAsync(string userId);
        Task<bool> IsEmailExistAsync(string email);
    }
}
