using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOS.AuthDTO;

namespace BLL.ServiceAbstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO> RegisterAdvisorAsync(RegisterAdvisorDTO dto);
        Task<AuthResponseDTO> RegisterAdminAsync(RegisterAdminDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
        Task<CurrentUserDTO> GetCurrentUserAsync(string email);
        Task<bool> IsEmailExistAsync(string email);
    }
}
