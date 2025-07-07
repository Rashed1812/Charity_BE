using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class AdvisorAvailabilityRepository : GenericRepository<AdvisorAvailability>, IAdvisorAvailabilityRepository
    {
        private readonly ApplicationDbContext _context;
        public AdvisorAvailabilityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<AdvisorAvailability>> GetByAdvisorIdAsync(int advisorId)
        {
            return await _context.AdvisorAvailabilities
                .Where(aa => aa.AdvisorId == advisorId && aa.IsAvailable)
                .OrderBy(aa => aa.DayOfWeek)
                .ThenBy(aa => aa.StartTime)
                .ToListAsync();
        }

        public async Task<List<AdvisorAvailability>> GetAvailableSlotsAsync(int advisorId, DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            return await _context.AdvisorAvailabilities
                .Where(aa => aa.AdvisorId == advisorId && 
                           aa.DayOfWeek == dayOfWeek && 
                           aa.IsAvailable)
                .OrderBy(aa => aa.StartTime)
                .ToListAsync();
        }

        public async Task<bool> IsSlotAvailableAsync(int advisorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var dayOfWeek = date.DayOfWeek;
            var availability = await _context.AdvisorAvailabilities
                .FirstOrDefaultAsync(aa => aa.AdvisorId == advisorId && 
                                         aa.DayOfWeek == dayOfWeek && 
                                         aa.IsAvailable &&
                                         aa.StartTime <= startTime && 
                                         aa.EndTime >= endTime);

            return availability != null;
        }

        public async Task<int> GetTotalAvailabilityByAdvisorAsync(int advisorId)
        {
            return await _context.AdvisorAvailabilities
                .Where(aa => aa.AdvisorId == advisorId && aa.IsAvailable)
                .CountAsync();
        }

        public async Task<int> GetBookedAvailabilityByAdvisorAsync(int advisorId)
        {
            // This would need to be implemented based on your booking logic
            // For now, returning 0 as placeholder
            return 0;
        }
    }
} 