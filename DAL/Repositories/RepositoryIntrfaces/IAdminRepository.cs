using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
    }
}
