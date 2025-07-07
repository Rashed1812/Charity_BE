using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.UserDTO;
using DAL.Data;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext dbcontext, UserManager<ApplicationUser> userManager) : base(dbcontext)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        public async void DeleteUserAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
            }
            var user = _dbcontext.Users.Find(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            _dbcontext.Users.Remove(user);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
            }
            return await _dbcontext.Users.FindAsync(id);
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _dbcontext.Users.Update(user);
            await _dbcontext.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(string id, UpdateUserDTO updateDto)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));

            var user = await _dbcontext.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");
            
            if (!string.IsNullOrEmpty(updateDto.FullName))
                user.FullName = updateDto.FullName;
            if (!string.IsNullOrEmpty(updateDto.PhoneNumber))
                user.PhoneNumber = updateDto.PhoneNumber;
            if (!string.IsNullOrEmpty(updateDto.Address))
                user.Address = updateDto.Address;
                
            _dbcontext.Users.Update(user);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _dbcontext.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetActiveUsersAsync()
        {
            return await _dbcontext.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _dbcontext.Users.CountAsync();
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _dbcontext.Users.CountAsync(u => u.IsActive);
        }

        public async Task<int> GetNewUsersThisMonthAsync()
        {
            var now = DateTime.UtcNow;
            return await _dbcontext.Users.CountAsync(u => u.CreatedAt.Value.Month == now.Month && u.CreatedAt.Value.Year == now.Year);
        }

        public async Task<int> GetNewUsersThisWeekAsync()
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
            return await _dbcontext.Users.CountAsync(u => u.CreatedAt >= startOfWeek);
        }

        public async Task<List<object>> GetUsersByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _dbcontext.Users
                .Where(u => u.CreatedAt >= startDate)
                .GroupBy(u => new { u.CreatedAt.Value.Year, u.CreatedAt.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetUsersByRoleCountAsync()
        {
            // This requires a join with Identity tables, so here is a placeholder
            return new List<object>();
        }

        public async Task<List<object>> GetTopActiveUsersAsync(int count)
        {
            // Placeholder: implement logic based on activity
            return new List<object>();
        }

        public async Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm)
        {
            return await _dbcontext.Users
                .Where(u => (u.FullName).Contains(searchTerm) || u.Email.Contains(searchTerm) || u.UserName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.Where(u => u.IsActive).ToList();
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _dbcontext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbcontext.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsPhoneUniqueAsync(string phoneNumber)
        {
            return !await _dbcontext.Users
                .AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<List<object>> GetUsersCountByRoleAsync()
        {
            var roles = await _userManager.GetRolesAsync(null);
            var result = new List<object>();
            
            foreach (var role in roles)
            {
                var count = (await _userManager.GetUsersInRoleAsync(role)).Count;
                result.Add(new { Role = role, Count = count });
            }
            
            return result;
        }
    }
}
