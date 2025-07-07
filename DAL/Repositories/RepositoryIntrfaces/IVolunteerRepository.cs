using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IVolunteerRepository : IGenericRepository<VolunteerApplication>
    {
        Task<List<VolunteerApplication>> GetAllApplicationsWithUserAsync();
        Task<VolunteerApplication> GetApplicationByUserAsync(string userId);
        Task<VolunteerApplication> GetApplicationByIdWithUserAsync(int id);
        Task<List<VolunteerApplication>> GetApplicationsByStatusAsync(VolunteerStatus status);
        Task<int> GetTotalApplicationsCountAsync();
        Task<int> GetApplicationsCountByStatusAsync(VolunteerStatus status);
        Task<List<object>> GetApplicationsByMonthAsync(int months);
        Task<List<object>> GetApplicationsByAreaOfInterestAsync();
        Task<double> GetAverageReviewTimeAsync();
        Task<List<VolunteerApplication>> GetRecentApplicationsAsync(int count);
        Task<bool> HasApplicationAsync(string userId);
        Task<List<VolunteerApplication>> GetVolunteerApplicationsByUserAsync(string userId);
    }
} 