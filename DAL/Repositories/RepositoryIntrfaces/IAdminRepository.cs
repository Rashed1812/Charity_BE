using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Task<List<Admin>> GetAllAdminsWithUserAsync();
        Task<Admin> GetAdminByIdWithUserAsync(int id);
        Task<Admin> GetAdminByUserIdAsync(string userId);
        Task<int> GetTotalAdminsCountAsync();
        Task<Admin> GetByUserIdAsync(string userId);
        Task<List<Admin>> GetActiveAdminsAsync();
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsPhoneUniqueAsync(string phoneNumber);
    }
}
