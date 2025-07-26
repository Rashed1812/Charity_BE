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
    public class MediationRepository : GenericRepository<Mediation>, IMediationRepository
    {
        private readonly ApplicationDbContext _context;
        public MediationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Mediation>> GetAllMediationsWithRelatedDataAsync()
        {
            return await _context.Mediations
                .Include(m => m.User)
                .ToListAsync();
        }

        public async Task<Mediation> GetMediationByIdWithRelatedDataAsync(int id)
        {
            return await _context.Mediations
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Mediation> GetMediationByUserIdAsync(string userId)
        {
            return await _context.Mediations
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }
    }
}
