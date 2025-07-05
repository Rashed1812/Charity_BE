using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOS.UserDTO;

namespace BLL.ServiceAbstraction
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<int> UpdateUserAsync(string id, UpdateUserDTO updateDto);
        Task<bool> DeleteUserAsync(string id);
    }
}
