using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositries.GenericRepositries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IReconcileRequestRepository : IGenericRepository<ReconcileRequest>
    {
        Task<List<ReconcileRequest>> GetByUserIdAsync(string userId);
    }
} 