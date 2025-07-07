using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AdminRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Admin>> GetAllAdminsWithUserAsync()
        {
            return await _dbContext.Admins.Include(a => a.User).ToListAsync();
        }

        public async Task<Admin> GetAdminByIdWithUserAsync(int id)
        {
            return await _dbContext.Admins.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Admin> GetAdminByUserIdAsync(string userId)
        {
            return await _dbContext.Admins.Include(a => a.User).FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<int> GetTotalAdminsCountAsync()
        {
            return await _dbContext.Admins.CountAsync();
        }

        public async Task<Admin> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<List<Admin>> GetActiveAdminsAsync()
        {
            return await _dbContext.Admins
                .Where(a => a.IsActive)
                .ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Admins
                .AnyAsync(a => a.Email == email);
        }

        public async Task<bool> IsPhoneUniqueAsync(string phoneNumber)
        {
            return !await _dbContext.Admins
                .AnyAsync(a => a.PhoneNumber == phoneNumber);
        }
    }
}
