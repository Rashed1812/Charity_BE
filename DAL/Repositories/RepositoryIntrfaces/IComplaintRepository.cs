using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;
using Shared.DTOS.ComplaintDTOs;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IComplaintRepository : IGenericRepository<Complaint>
    {
        Task<List<Complaint>> GetAllComplaintsWithUserAsync();
        Task<List<Complaint>> GetComplaintsByUserAsync(string userId);
        Task<int> GetTotalComplaintsCountAsync();
        Task<int> GetComplaintsCountByStatusAsync(ComplaintStatus status);
        Task<List<object>> GetComplaintsByMonthAsync(int months);
        Task<double> GetAverageResponseTimeAsync();
        Task<List<Complaint>> GetRecentComplaintsAsync(int count);
        Task<bool> HasActiveComplaintsAsync(string userId);
        Task<List<Complaint>> GetByUserIdAsync(string userId);
        Task<List<Complaint>> GetByStatusAsync(ComplaintStatus status);
        Task<List<Complaint>> GetByCategoryAsync(Shared.DTOS.ComplaintDTOs.ComplaintCategory category);
        Task<List<Complaint>> GetPendingComplaintsAsync();
        Task<List<Complaint>> GetResolvedComplaintsAsync();
        Task<int> GetComplaintCountByStatusAsync(ComplaintStatus status);
        Task<int> GetTotalComplaintsByUserAsync(string userId);
        Task<int> GetPendingComplaintsByUserAsync(string userId);
    }
} 