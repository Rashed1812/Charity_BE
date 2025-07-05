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
    public class AdvisorRepository : GenericRepository<Advisor> , IAdvisorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AdvisorRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<IEnumerable<Advisor>> GetAllWithIncludesAsync()
        {
            return await _dbContext.Advisors
                .Include(a => a.User)
                .Include(a => a.Consultation)
                .Include(a => a.Availabilities)
                .ToListAsync();
        }
        public async Task<Advisor> GetByIdWithIncludesAsync(int id)
        {
            return await _dbContext.Advisors
                .Include(a => a.User)
                .Include(a => a.Consultation)
                .Include(a => a.Availabilities)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
