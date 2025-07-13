using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class AdvisorRepository : GenericRepository<Advisor>, IAdvisorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AdvisorRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<Advisor> GetAdvisorByAvailabilityAsync(int availabilityId)
        {
            return await _dbContext.Advisors
                .Include(a => a.Availabilities)
                .FirstOrDefaultAsync(a => a.Availabilities.Any(av => av.Id == availabilityId));
        }

        public async Task<Advisor> GetAdvisorByConsultationAsync(int consultationId)
        {
            return await _dbContext.Advisors
                .Include(a => a.Consultation)
                .FirstOrDefaultAsync(a => a.Consultation.Id == consultationId);
        }

        public async Task<IEnumerable<Advisor>> GetAdvisorsByAvailabilityAsync(int availabilityId)
        {
            return await _dbContext.Advisors
                .Include(a => a.Availabilities)
                .Where(a => a.Availabilities.Any(av => av.Id == availabilityId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Advisor>> GetAdvisorsByConsultationAsync(int consultationId)
        {
            return await _dbContext.Advisors
                .Include(a => a.Consultation)
                .Where(a => a.Consultation.Id == consultationId)
                .ToListAsync();
        }

        public async Task<List<Advisor>> GetAllWithIncludesAsync()
        {
            return await _dbContext.Advisors
                .Include(a => a.User)
                .Include(a => a.Consultation)
                .Include(a => a.Availabilities.Select(av => new {av.Date, av.Time, av.IsBooked}))
                .ToListAsync();
        }
        public async Task<Advisor> GetByIdWithIncludesAsync(int id)
        {
            return await _dbContext.Advisors
                .Include(a => a.User)
                .Include(a => a.Consultation)
                .Include(a => a.Availabilities)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Advisor>> GetAllAdvisorsWithRelatedDataAsync()
        {
            return await _dbContext.Advisors
                .Include(a => a.User)
                .Include(a => a.Consultation)
                .Include(a => a.Availabilities)
                .ToListAsync();
        }

        public async Task<Advisor> GetAdvisorByIdWithRelatedDataAsync(int id)
        {
            return await _dbContext.Advisors
                .Include(a => a.User)
                .Include(a => a.Consultation)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<AdvisorAvailability>> GetAdvisorAvailabilityAsync(int advisorId)
        {
            return await _dbContext.AdvisorAvailabilities
                .Where(a => a.AdvisorId == advisorId)
                .OrderBy(a => a.Date).ThenBy(a => a.Time)
                .ToListAsync();
        }

        public async Task<AdvisorAvailability> GetAvailabilityByIdAsync(int availabilityId)
        {
            return await _dbContext.AdvisorAvailabilities.FirstOrDefaultAsync(a => a.Id == availabilityId);
        }

        public async Task<AdvisorAvailability> GetAvailabilityByDateTimeAsync(int advisorId, DateTime dateTime)
        {
            return await _dbContext.AdvisorAvailabilities.FirstOrDefaultAsync(a => 
                a.AdvisorId == advisorId && 
                a.Date == dateTime.Date && 
                a.Time <= dateTime.TimeOfDay && 
                a.Time + a.Duration > dateTime.TimeOfDay);
        }

        public async Task<AdvisorAvailability> AddAvailabilityAsync(AdvisorAvailability availability)
        {
            _dbContext.AdvisorAvailabilities.Add(availability);
            await _dbContext.SaveChangesAsync();
            return availability;
        }

        public async Task<AdvisorAvailability> UpdateAvailabilityAsync(AdvisorAvailability availability)
        {
            _dbContext.AdvisorAvailabilities.Update(availability);
            await _dbContext.SaveChangesAsync();
            return availability;
        }

        public async Task<bool> DeleteAvailabilityAsync(int availabilityId)
        {
            var availability = await _dbContext.AdvisorAvailabilities.FindAsync(availabilityId);
            if (availability == null) return false;
            _dbContext.AdvisorAvailabilities.Remove(availability);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalAvailabilityByAdvisorAsync(int advisorId)
        {
            return await _dbContext.AdvisorAvailabilities.CountAsync(a => a.AdvisorId == advisorId);
        }

        public async Task<int> GetBookedAvailabilityByAdvisorAsync(int advisorId)
        {
            return await _dbContext.AdvisorAvailabilities.CountAsync(a => a.AdvisorId == advisorId && !a.IsBooked);
        }

        public async Task<int> GetAdvisorsCountByConsultationAsync(int consultationId)
        {
            return await _dbContext.Advisors.CountAsync(a => a.ConsultationId == consultationId);
        }

        public async Task<int> GetActiveAdvisorsCountByConsultationAsync(int consultationId)
        {
            return await _dbContext.Advisors.CountAsync(a => a.ConsultationId == consultationId && a.IsActive);
        }

        public async Task<List<object>> GetTopAdvisorsByConsultationAsync(int consultationId, int count)
        {
            return await _dbContext.Advisors
                .Where(a => a.ConsultationId == consultationId && a.IsActive)
                .Select(a => new {
                    a.Id,
                    FullName = a.FullName ,
                    a.Specialty,
                    a.Description,
                    a.ZoomRoomUrl,
                    a.IsActive
                })
                .Take(count)
                .ToListAsync<object>();
        }

        public async Task<Advisor> GetAdvisorsByUserIdAsync(string userId)
        {
            return await _dbContext.Advisors
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }
    }
}
