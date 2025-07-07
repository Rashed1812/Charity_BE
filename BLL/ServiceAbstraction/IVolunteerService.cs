using DAL.Data.Models;
using Shared.DTOS.VolunteerDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IVolunteerService
    {
        // Application Management
        Task<List<VolunteerApplicationDTO>> GetAllApplicationsAsync();
        Task<VolunteerApplicationDTO> GetUserApplicationAsync(string userId);
        Task<VolunteerApplicationDTO> GetApplicationByIdAsync(int id);
        Task<VolunteerApplicationDTO> CreateApplicationAsync(string userId, CreateVolunteerApplicationDTO createApplicationDto);
        Task<VolunteerApplicationDTO> UpdateApplicationAsync(int id, string userId, UpdateVolunteerApplicationDTO updateApplicationDto);
        Task<bool> DeleteApplicationAsync(int id);

        // Review Management
        Task<VolunteerApplicationDTO> ReviewApplicationAsync(int id, string adminId, ReviewVolunteerApplicationDTO reviewDto);
        Task<List<VolunteerApplicationDTO>> GetApplicationsByStatusAsync(VolunteerStatus status);

        // Statistics
        Task<object> GetVolunteerStatisticsAsync();
    }
} 