using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryClasses
{
    public class ReconcileRequestRepository : GenericRepository<ReconcileRequest>, IReconcileRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public ReconcileRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ReconcileRequest>> GetByUserIdAsync(string userId)
        {
            return await _context.ReconcileRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
} 