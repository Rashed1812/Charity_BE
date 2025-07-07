using Shared.DTOS.AdminDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IAdminService
    {
        Task<List<AdminDTO>> GetAllAdminsAsync();
        Task<AdminDTO> GetAdminByIdAsync(int id);
        Task<AdminDTO> GetAdminByUserIdAsync(string userId);
        Task<AdminDTO> CreateAdminAsync(CreateAdminDTO createAdminDto);
        Task<AdminDTO> UpdateAdminAsync(int id, UpdateAdminDTO updateAdminDto);
        Task<bool> DeleteAdminAsync(int id);
        Task<object> GetSystemStatisticsAsync();
        Task<object> GetDashboardDataAsync();
        Task<bool> ApproveUserAsync(string userId);
        Task<bool> RejectUserAsync(string userId, string reason);
        Task<bool> SuspendUserAsync(string userId, string reason, DateTime? until);
        Task<bool> ActivateUserAsync(string userId);
        Task<List<object>> GetSystemLogsAsync(DateTime? fromDate, DateTime? toDate, string logLevel);
        Task<bool> SendSystemNotificationAsync(string title, string message, List<string> userIds);
    }
} 