using DAL.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IHelpRequestRepository
    {
        Task<List<HelpRequest>> GetAllAsync();
        Task<HelpRequest> AddAsync(HelpRequest entity);
    }
} 