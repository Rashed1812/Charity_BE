using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.UserDTO;
using DAL.Data.Models.IdentityModels;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<List<ApplicationUser>> GetActiveUsersAsync();
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetNewUsersThisMonthAsync();
        Task<int> GetNewUsersThisWeekAsync();
        Task<List<object>> GetUsersByMonthAsync(int months);
        Task<List<object>> GetUsersByRoleCountAsync();
        Task<List<object>> GetTopActiveUsersAsync(int count);
        Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
        Task<ApplicationUser> GetByIdAsync(string id);
        new Task<ApplicationUser> UpdateAsync(ApplicationUser user);
        Task UpdateUserAsync(string id, UpdateUserDTO updateDto);
        void DeleteUserAsync(string id);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsPhoneUniqueAsync(string phoneNumber);
        Task<List<object>> GetUsersCountByRoleAsync();
    }
}
