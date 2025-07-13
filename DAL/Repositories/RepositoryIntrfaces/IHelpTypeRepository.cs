using DAL.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IHelpTypeRepository
    {
        Task<List<HelpType>> GetAllAsync();
        Task<HelpType> GetByIdAsync(int id);
        Task<HelpType> AddAsync(HelpType entity);
        Task<HelpType> UpdateAsync(HelpType entity);
        Task<bool> DeleteAsync(HelpType entity);
    }
} 