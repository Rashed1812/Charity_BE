using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class MediationRepository : GenericRepository<Mediation>, IMediationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MediationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Mediation> GetMediationByUserIdAsync(string userId)
        {
            return await _dbContext.Mediations.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<List<Mediation>> GetAllMediationsWithRelatedDataAsync()
        {
            return await _dbContext.Mediations.Include(m => m.User).ToListAsync();
        }

        public async Task<Mediation> GetMediationByIdWithRelatedDataAsync(int id)
        {
            return await _dbContext.Mediations.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
} 