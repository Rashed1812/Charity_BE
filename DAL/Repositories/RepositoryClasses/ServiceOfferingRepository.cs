using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class ServiceOfferingRepository : GenericRepository<ServiceOffering>, IServiceOfferingRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceOfferingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ServiceOffering>> GetAllServicesWithProviderAsync()
        {
            return await _context.ServiceOfferings
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ServiceOffering>> GetActiveServicesWithProviderAsync()
        {
            return await _context.ServiceOfferings
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceOffering> GetServiceByIdWithProviderAsync(int id)
        {
            return await _context.ServiceOfferings
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<ServiceOffering>> GetServicesByProviderAsync(string providerId)
        {
            // Since there's no ProviderId in the model, return empty list or implement based on your needs
            return new List<ServiceOffering>();
        }

        public async Task<List<ServiceOffering>> GetServicesByCategoryAsync(string category)
        {
            return await _context.ServiceOfferings
                .Where(s => s.Category == category && s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ServiceOffering>> SearchServicesAsync(string searchTerm)
        {
            return await _context.ServiceOfferings
                .Where(s => s.IsActive && (s.Name.Contains(searchTerm) || s.Description.Contains(searchTerm)))
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ServiceOffering>> GetServicesByLocationAsync(string location)
        {
            // Since there's no Location in the model, return empty list or implement based on your needs
            return new List<ServiceOffering>();
        }

        public async Task<int> GetTotalServicesCountAsync()
        {
            return await _context.ServiceOfferings.CountAsync();
        }

        public async Task<int> GetActiveServicesCountAsync()
        {
            return await _context.ServiceOfferings.CountAsync(s => s.IsActive);
        }

        public async Task<int> GetInactiveServicesCountAsync()
        {
            return await _context.ServiceOfferings.CountAsync(s => !s.IsActive);
        }

        public async Task<List<object>> GetServicesByCategoryCountAsync()
        {
            return await _context.ServiceOfferings
                .GroupBy(s => s.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetServicesByLocationCountAsync()
        {
            // Since there's no Location in the model, return empty list
            return new List<object>();
        }

        public async Task<List<object>> GetServicesByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _context.ServiceOfferings
                .Where(s => s.CreatedAt >= startDate)
                .GroupBy(s => new { s.CreatedAt.Year, s.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetTopProvidersAsync(int count)
        {
            // Since there's no Provider in the model, return empty list
            return new List<object>();
        }

        public async Task<List<string>> GetServiceCategoriesAsync()
        {
            return await _context.ServiceOfferings
                .Where(s => s.IsActive)
                .Select(s => s.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetServiceLocationsAsync()
        {
            // Since there's no Location in the model, return empty list
            return new List<string>();
        }

        public async Task<bool> HasServicesAsync(string providerId)
        {
            // Since there's no ProviderId in the model, return false
            return false;
        }

        public async Task<List<ServiceOffering>> GetActiveServicesAsync()
        {
            return await _context.ServiceOfferings
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<ServiceOffering>> GetByCategoryAsync(string category)
        {
            return await _context.ServiceOfferings
                .Where(s => s.Category == category && s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<ServiceOffering>> GetMostClickedServicesAsync(int count)
        {
            return await _context.ServiceOfferings
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.ClickCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetTotalClickCountAsync()
        {
            return await _context.ServiceOfferings
                .SumAsync(s => s.ClickCount);
        }
    }
} 