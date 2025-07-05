using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOS.UserDTO;
using DAL.Data;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;

namespace DAL.Repositories.RepositoryClasses
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public UserRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
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

        public async Task UpdateUserAsync(string id, UpdateUserDTO updateDto)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));

            var user = await _dbcontext.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");
            user.FullName = updateDto.FullName ?? user.FullName;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
            _dbcontext.Users.Update(user);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
