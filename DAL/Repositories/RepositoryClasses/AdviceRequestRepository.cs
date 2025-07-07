using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.AdvisorDTOs;

namespace DAL.Repositories.RepositoryClasses
{
    public class AdviceRequestRepository : GenericRepository<AdviceRequest>, IAdviceRequestRepository
    {
        private readonly ApplicationDbContext _context; 
        public AdviceRequestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<AdviceRequest>> GetAllRequestsWithDetailsAsync()
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetRequestsByUserAsync(string userId)
        {
            return await _context.AdviceRequests
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<AdviceRequest> GetRequestByIdWithDetailsAsync(int id)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<AdviceRequest>> GetRequestsByStatusAsync(ConsultationStatus status)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetRequestsByAdvisorAsync(int advisorId)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Consultation)
                .Where(r => r.AdvisorId == advisorId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetRequestsByConsultationAsync(int consultationId)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Where(r => r.ConsultationId == consultationId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalRequestsCountAsync()
        {
            return await _context.AdviceRequests.CountAsync();
        }

        public async Task<int> GetPendingRequestsCountAsync()
        {
            return await _context.AdviceRequests.CountAsync(r => r.Status == ConsultationStatus.Pending);
        }

        public async Task<int> GetConfirmedRequestsCountAsync()
        {
            return await _context.AdviceRequests.CountAsync(r => r.Status == ConsultationStatus.Confirmed);
        }

        public async Task<int> GetCompletedRequestsCountAsync()
        {
            return await _context.AdviceRequests.CountAsync(r => r.Status == ConsultationStatus.Completed);
        }

        public async Task<int> GetCancelledRequestsCountAsync()
        {
            return await _context.AdviceRequests.CountAsync(r => r.Status == ConsultationStatus.Cancelled);
        }

        public async Task<int> GetRequestsCountByConsultationAsync(int consultationId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.ConsultationId == consultationId);
        }

        public async Task<int> GetPendingRequestsCountByConsultationAsync(int consultationId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.ConsultationId == consultationId && r.Status == ConsultationStatus.Pending);
        }

        public async Task<int> GetCompletedRequestsCountByConsultationAsync(int consultationId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.ConsultationId == consultationId && r.Status == ConsultationStatus.Completed);
        }

        public async Task<List<object>> GetRequestsByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _context.AdviceRequests
                .Where(r => r.CreatedAt >= startDate)
                .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetRequestsByConsultationAndMonthAsync(int consultationId, int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _context.AdviceRequests
                .Where(r => r.ConsultationId == consultationId && r.CreatedAt >= startDate)
                .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetTopAdvisorsAsync(int count)
        {
            return await _context.AdviceRequests
                .Where(r => r.Status == ConsultationStatus.Completed)
                .GroupBy(r => r.AdvisorId)
                .Select(g => new
                {
                    AdvisorId = g.Key,
                    AdvisorName = g.First().Advisor.FullName,
                    CompletedConsultations = g.Count(),
                    TotalConsultations = _context.AdviceRequests.Count(r => r.AdvisorId == g.Key)
                })
                .OrderByDescending(x => x.CompletedConsultations)
                .Take(count)
                .ToListAsync<object>();
        }

        public async Task<bool> HasActiveConsultationsAsync(string userId)
        {
            return await _context.AdviceRequests
                .AnyAsync(r => r.UserId == userId && 
                              (r.Status == ConsultationStatus.Pending || r.Status == ConsultationStatus.Confirmed));
        }

        public async Task<List<AdviceRequest>> GetRecentConsultationsAsync(int count)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetActiveRequestsByAdvisorAsync(int advisorId)
        {
            return await _context.AdviceRequests
                .Where(r => r.AdvisorId == advisorId && 
                           (r.Status == ConsultationStatus.Pending || r.Status == ConsultationStatus.Confirmed))
                .ToListAsync();
        }

        public async Task<int> GetTotalConsultationsByAdvisorAsync(int advisorId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.AdvisorId == advisorId);
        }

        public async Task<int> GetPendingRequestsByAdvisorAsync(int advisorId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.AdvisorId == advisorId && r.Status == ConsultationStatus.Pending);
        }

        public async Task<int> GetCompletedConsultationsByAdvisorAsync(int advisorId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.AdvisorId == advisorId && r.Status == ConsultationStatus.Completed);
        }

        public async Task<List<AdviceRequest>> GetByUserIdAsync(string userId)
        {
            return await _context.AdviceRequests
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetByAdvisorIdAsync(int advisorId)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Consultation)
                .Where(r => r.AdvisorId == advisorId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetByConsultationIdAsync(int consultationId)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Where(r => r.ConsultationId == consultationId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetByStatusAsync(ConsultationStatus status)
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetPendingRequestsAsync()
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .Where(r => r.Status == ConsultationStatus.Pending)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdviceRequest>> GetCompletedRequestsAsync()
        {
            return await _context.AdviceRequests
                .Include(r => r.User)
                .Include(r => r.Advisor)
                .Include(r => r.Consultation)
                .Where(r => r.Status == ConsultationStatus.Completed)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalConsultationsByUserAsync(string userId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.UserId == userId);
        }

        public async Task<int> GetPendingConsultationsByUserAsync(string userId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.UserId == userId && r.Status == ConsultationStatus.Pending);
        }

        public async Task<int> GetCompletedConsultationsByUserAsync(string userId)
        {
            return await _context.AdviceRequests.CountAsync(r => r.UserId == userId && r.Status == ConsultationStatus.Completed);
        }
    }
} 