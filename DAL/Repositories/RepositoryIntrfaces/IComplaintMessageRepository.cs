using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IComplaintMessageRepository : IGenericRepository<ComplaintMessage>
    {
        Task<List<ComplaintMessage>> GetByComplaintIdAsync(int complaintId);
        Task<List<ComplaintMessage>> GetByUserIdAsync(string userId);
        Task<int> GetMessageCountByComplaintAsync(int complaintId);

    }
} 