using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IVolunteerApplicationRepository : IGenericRepository<VolunteerApplication>
    {
        Task<VolunteerApplication> GetByUserIdAsync(string userId);
        Task<List<VolunteerApplication>> GetByStatusAsync(VolunteerStatus status);
        Task<List<VolunteerApplication>> GetPendingApplicationsAsync();
        Task<List<VolunteerApplication>> GetApprovedApplicationsAsync();
        Task<int> GetApplicationCountByStatusAsync(VolunteerStatus status);
    }
} 