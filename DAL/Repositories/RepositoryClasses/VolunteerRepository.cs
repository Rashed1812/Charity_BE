using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class VolunteerRepository : GenericRepository<VolunteerApplication>, IVolunteerRepository
    {
        private readonly ApplicationDbContext _context;
        public VolunteerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<VolunteerApplication>> GetAllApplicationsWithUserAsync()
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<VolunteerApplication> GetApplicationByUserAsync(string userId)
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.UserId == userId);
        }

        public async Task<VolunteerApplication> GetApplicationByIdWithUserAsync(int id)
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<VolunteerApplication>> GetApplicationsByStatusAsync(VolunteerStatus status)
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .Where(v => v.Status == status)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalApplicationsCountAsync()
        {
            return await _context.VolunteerApplications.CountAsync();
        }

        public async Task<int> GetApplicationsCountByStatusAsync(VolunteerStatus status)
        {
            return await _context.VolunteerApplications.CountAsync(v => v.Status == status);
        }

        public async Task<List<object>> GetApplicationsByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _context.VolunteerApplications
                .Where(v => v.CreatedAt >= startDate)
                .GroupBy(v => new { v.CreatedAt.Year, v.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<List<VolunteerApplication>> GetRecentApplicationsAsync(int count)
        {
            return await _context.VolunteerApplications
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> HasApplicationAsync(string userId)
        {
            return await _context.VolunteerApplications.AnyAsync(v => v.UserId == userId);
        }

        public async Task<List<VolunteerApplication>> GetVolunteerApplicationsByUserAsync(string userId)
        {
            return await _context.VolunteerApplications
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }
    }
} 