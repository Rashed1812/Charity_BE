using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.UserDTO;

namespace BLL.ServiceAbstraction
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<List<UserDTO>> GetActiveUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> UpdateUserAsync(string id, UpdateUserDTO updateUserDto);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
        Task<bool> DeleteUserAsync(string id);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<object> GetUserDashboardAsync(string userId);
        Task<object> GetUserStatisticsAsync();
        Task<List<UserDTO>> SearchUsersAsync(string searchTerm);
        Task<List<UserDTO>> GetUsersByRoleAsync(string role);
        Task<bool> UpdateUserProfilePictureAsync(string userId, string profilePictureUrl);
        Task<bool> VerifyEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ActivateUserAsync(string id);
        Task<bool> DeactivateUserAsync(string id);
    }
}
