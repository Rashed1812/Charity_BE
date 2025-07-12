using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.AdvisorDTOs;

namespace DAL.Repositories.RepositoryClasses
{
    public class AdvisorAvailabilityRepository : GenericRepository<AdvisorAvailability>, IAdvisorAvailabilityRepository
    {
        private readonly ApplicationDbContext _context;
        public AdvisorAvailabilityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AdvisorAvailability>> GetByAdvisorIdAsync(int advisorId)
        {
            return await _context.AdvisorAvailabilities
                .Where(aa => aa.AdvisorId == advisorId && !aa.IsBooked)
                .OrderBy(aa => aa.Date)
                .ThenBy(aa => aa.Time)
                .ToListAsync();
        }

        public async Task<List<AdvisorAvailability>> GetAvailableSlotsAsync(int advisorId, DateTime date)
        {
            return await _context.AdvisorAvailabilities
                .Where(aa => aa.AdvisorId == advisorId && 
                           aa.Date == date && 
                           !aa.IsBooked)
                .OrderBy(aa => aa.Time)
                .ToListAsync();
        }

        public async Task<bool> IsSlotAvailableAsync(int advisorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var availability = await _context.AdvisorAvailabilities
                .FirstOrDefaultAsync(aa => aa.AdvisorId == advisorId && 
                                         aa.Date == date && 
                                         !aa.IsBooked &&
                                         aa.Time <= startTime && 
                                         aa.Time >= endTime);

            return availability != null;
        }

        public async Task<int> GetTotalAvailabilityByAdvisorAsync(int advisorId)
        {
            return await _context.AdvisorAvailabilities
                .Where(aa => aa.AdvisorId == advisorId && !aa.IsBooked)
                .CountAsync();
        }

        public async Task<int> GetBookedAvailabilityByAdvisorAsync(int advisorId)
        {
            // This would need to be implemented based on your booking logic
            // For now, returning 0 as placeholder
            return 0;
        }
        public async Task<List<AdvisorAvailability>> GetAvailableSlotsForDayAsync(int advisorId, DateTime date)
        {
            var availabilities = await _context.AdvisorAvailabilities
                .Where(aa => aa.AdvisorId == advisorId &&
                             aa.Date == date &&
                             !aa.IsBooked)
                .ToListAsync();


            var bookedTimes = await _context.AdviceRequests
                .Where(r => r.AdvisorId == advisorId &&
                            r.CreatedAt.Date == date.Date &&
                            (r.Status == ConsultationStatus.Pending || r.Status == ConsultationStatus.Confirmed))
                .Select(r => r.CreatedAt.TimeOfDay)
                .ToListAsync();


            var availableSlots = availabilities
                .Where(a => !bookedTimes.Any(bt => bt >= a.Time && bt < a.Time))
                .ToList();

            return availableSlots;
        }
    }
} 