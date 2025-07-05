using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;

namespace DAL.Repositories.RepositoryClasses
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AdminRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
