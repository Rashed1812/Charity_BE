using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.ComplaintDTOs;

namespace DAL.Repositories.RepositoryClasses
{
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        private readonly ApplicationDbContext _context;
        public ComplaintRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Complaint>> GetAllComplaintsWithUserAsync()
        {
            return await _context.Complaints
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetComplaintsByUserAsync(string userId)
        {
            return await _context.Complaints
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Complaint> GetComplaintByIdWithMessagesAsync(int id)
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> GetTotalComplaintsCountAsync()
        {
            return await _context.Complaints.CountAsync();
        }

        public async Task<int> GetComplaintsCountByStatusAsync(ComplaintStatus status)
        {
            return await _context.Complaints.CountAsync(c => c.Status == status);
        }

        public async Task<List<object>> GetComplaintsByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _context.Complaints
                .Where(c => c.CreatedAt >= startDate)
                .GroupBy(c => new { c.CreatedAt.Year, c.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<double> GetAverageResponseTimeAsync()
        {
            var complaints = await _context.Complaints
                .Where(c => c.ResolvedAt.HasValue)
                .Select(c => (c.ResolvedAt.Value - c.CreatedAt).TotalHours)
                .ToListAsync();

            return complaints.Any() ? complaints.Average() : 0;
        }

        public async Task<List<Complaint>> GetRecentComplaintsAsync(int count)
        {
            return await _context.Complaints
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> HasActiveComplaintsAsync(string userId)
        {
            return await _context.Complaints
                .AnyAsync(c => c.UserId == userId && 
                              (c.Status == ComplaintStatus.Pending || c.Status == ComplaintStatus.InProgress));
        }

        public async Task<List<Complaint>> GetByUserIdAsync(string userId)
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetByStatusAsync(ComplaintStatus status)
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Where(c => c.Status == status)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetByCategoryAsync(string category)
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Where(c => c.Category == category)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetPendingComplaintsAsync()
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Where(c => c.Status == ComplaintStatus.Pending)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetResolvedComplaintsAsync()
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Where(c => c.Status == ComplaintStatus.Resolved)
                .ToListAsync();
        }

        public async Task<int> GetComplaintCountByStatusAsync(ComplaintStatus status)
        {
            return await _context.Complaints
                .CountAsync(c => c.Status == status);
        }

        public async Task<int> GetTotalComplaintsByUserAsync(string userId)
        {
            return await _context.Complaints.CountAsync(c => c.UserId == userId);
        }

        public async Task<int> GetPendingComplaintsByUserAsync(string userId)
        {
            return await _context.Complaints.CountAsync(c => c.UserId == userId && c.Status == ComplaintStatus.Pending);
        }
    }
} 