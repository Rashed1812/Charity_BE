using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class VolunteerApplicationRepository : GenericRepository<VolunteerApplication>, IVolunteerApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public VolunteerApplicationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<VolunteerApplication> GetByUserIdAsync(string userId)
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.UserId == userId);
        }

        public async Task<List<VolunteerApplication>> GetByStatusAsync(VolunteerStatus status)
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .Where(v => v.Status == status)
                .ToListAsync();
        }

        public async Task<List<VolunteerApplication>> GetPendingApplicationsAsync()
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .Where(v => v.Status == VolunteerStatus.Pending)
                .ToListAsync();
        }

        public async Task<List<VolunteerApplication>> GetApprovedApplicationsAsync()
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .Where(v => v.Status == VolunteerStatus.Approved)
                .ToListAsync();
        }

        public async Task<int> GetApplicationCountByStatusAsync(VolunteerStatus status)
        {
            return await _context.VolunteerApplications
                .CountAsync(v => v.Status == status);
        }
    }
} 