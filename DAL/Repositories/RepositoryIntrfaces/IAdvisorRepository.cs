using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;
using DAL.Repositries.GenericRepositries;
using DAL.Data.Models;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IAdvisorRepository : IGenericRepository<Advisor>
    {
        Task<List<Advisor>> GetAllAdvisorsWithRelatedDataAsync();
        Task<Advisor> GetAdvisorByIdWithRelatedDataAsync(int id);
        Task<IEnumerable<Advisor>> GetAdvisorsByConsultationAsync(int consultationId);
        Task<List<AdvisorAvailability>> GetAdvisorAvailabilityAsync(int advisorId);
        Task<AdvisorAvailability> GetAvailabilityByIdAsync(int availabilityId);
        Task<AdvisorAvailability> GetAvailabilityByDateTimeAsync(int advisorId, DateTime dateTime);
        Task<AdvisorAvailability> AddAvailabilityAsync(AdvisorAvailability availability);
        Task<AdvisorAvailability> UpdateAvailabilityAsync(AdvisorAvailability availability);
        Task<bool> DeleteAvailabilityAsync(int availabilityId);
        Task<int> GetTotalAvailabilityByAdvisorAsync(int advisorId);
        Task<int> GetBookedAvailabilityByAdvisorAsync(int advisorId);
        Task<int> GetAdvisorsCountByConsultationAsync(int consultationId);
        Task<int> GetActiveAdvisorsCountByConsultationAsync(int consultationId);
        Task<List<object>> GetTopAdvisorsByConsultationAsync(int consultationId, int count);
        Task<Advisor> GetAdvisorsByUserIdAsync(string userId);
    }
}
