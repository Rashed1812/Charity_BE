using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IAdvisorAvailabilityRepository : IGenericRepository<AdvisorAvailability>
    {
        Task<List<AdvisorAvailability>> GetByAdvisorIdAsync(int advisorId);
        Task<List<AdvisorAvailability>> GetAvailableSlotsAsync(int advisorId, DateTime date);
        Task<bool> IsSlotAvailableAsync(int advisorId, DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<int> GetTotalAvailabilityByAdvisorAsync(int advisorId);
        Task<int> GetBookedAvailabilityByAdvisorAsync(int advisorId);
    }
} 