using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class ComplaintMessageRepository : GenericRepository<ComplaintMessage>, IComplaintMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public ComplaintMessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ComplaintMessage>> GetByComplaintIdAsync(int complaintId)
        {
            return await _context.ComplaintMessages
                .Include(cm => cm.Complaint)
                .Include(cm => cm.User)
                .Where(cm => cm.ComplaintId == complaintId)
                .OrderBy(cm => cm.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ComplaintMessage>> GetByUserIdAsync(string userId)
        {
            return await _context.ComplaintMessages
                .Include(cm => cm.Complaint)
                .Include(cm => cm.User)
                .Where(cm => cm.UserId == userId)
                .OrderByDescending(cm => cm.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetMessageCountByComplaintAsync(int complaintId)
        {
            return await _context.ComplaintMessages
                .CountAsync(cm => cm.ComplaintId == complaintId);
        }
    }
} 