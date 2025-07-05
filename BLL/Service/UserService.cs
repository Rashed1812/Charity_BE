using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOS.UserDTO;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;

namespace BLL.Service
{
    public class UserService(IUserRepository _userRepository) : IUserService
    {
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName,
            }).ToList();
        }
        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
            }
             _userRepository.DeleteUserAsync(id);
            var result =  await _userRepository.SaveChangesAsync();
            if(result < 0)
                return false;
            return true;
        }


        public async Task<int> UpdateUserAsync(string id, UpdateUserDTO updateDto)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto), "Update data cannot be null.");

           
            await _userRepository.UpdateUserAsync(id, updateDto);
            var result = await _userRepository.SaveChangesAsync();
            if (result < 0)
                throw new Exception("Failed to update user.");
            return result;
        }
    }
}
