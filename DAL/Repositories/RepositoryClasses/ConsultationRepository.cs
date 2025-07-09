using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class ConsultationRepository : GenericRepository<Consultation>, IConsultationRepository
    {
        private readonly ApplicationDbContext _context;
        public ConsultationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Consultation>> GetActiveConsultationsAsync()
        {
            return await _context.Consultations
                .Where(c => c.IsActive)
                .OrderBy(c => c.ConsultationName)
                .ToListAsync();
        }

        public async Task<Consultation> GetByNameAsync(string name)
        {
            return await _context.Consultations
                .FirstOrDefaultAsync(c => c.ConsultationName == name);
        }

        public async Task<int> GetAdvisorCountAsync(int consultationId)
        {
            return await _context.Advisors
                .CountAsync(a => a.ConsultationId == consultationId && a.IsActive);
        }

        public async Task<int> GetRequestCountAsync(int consultationId)
        {
            return await _context.AdviceRequests
                .CountAsync(ar => ar.ConsultationId == consultationId);
        }

        public async Task<int> GetLectureCountAsync(int consultationId)
        {
            return await _context.Lectures
                .CountAsync(l => l.ConsultationId == consultationId && l.IsPublished);
        }

        public async Task<IEnumerable<Consultation>> GetAllWithIncludesAsync()
        {
            return await Task.FromResult(_context.Consultations
                .Include(c => c.Advisors)
                .Include(c => c.AdviceRequests)
                .AsEnumerable());

        }

        public async Task<Consultation> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Consultations
                .Include(c => c.Advisors)
                .Include(c => c.AdviceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
} 