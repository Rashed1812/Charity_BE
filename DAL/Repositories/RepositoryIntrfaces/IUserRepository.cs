using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOS.UserDTO;
using DAL.Data.Models.IdentityModels;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByIdAsync(string id);
        Task UpdateUserAsync(string id, UpdateUserDTO updateDto);
        void DeleteUserAsync(string id);
    }
}
