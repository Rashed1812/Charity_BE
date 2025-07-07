using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IServiceOfferingRepository : IGenericRepository<ServiceOffering>
    {
        Task<List<ServiceOffering>> GetAllServicesWithProviderAsync();
        Task<List<ServiceOffering>> GetActiveServicesWithProviderAsync();
        Task<ServiceOffering> GetServiceByIdWithProviderAsync(int id);
        Task<List<ServiceOffering>> GetServicesByProviderAsync(string providerId);
        Task<List<ServiceOffering>> GetServicesByCategoryAsync(string category);
        Task<List<ServiceOffering>> SearchServicesAsync(string searchTerm);
        Task<List<ServiceOffering>> GetServicesByLocationAsync(string location);
        Task<int> GetTotalServicesCountAsync();
        Task<int> GetActiveServicesCountAsync();
        Task<int> GetInactiveServicesCountAsync();
        Task<List<object>> GetServicesByCategoryCountAsync();
        Task<List<object>> GetServicesByLocationCountAsync();
        Task<List<object>> GetServicesByMonthAsync(int months);
        Task<List<object>> GetTopProvidersAsync(int count);
        Task<List<string>> GetServiceCategoriesAsync();
        Task<List<string>> GetServiceLocationsAsync();
        Task<bool> HasServicesAsync(string providerId);
        Task<List<ServiceOffering>> GetActiveServicesAsync();
        Task<List<ServiceOffering>> GetByCategoryAsync(string category);
        Task<List<ServiceOffering>> GetMostClickedServicesAsync(int count);
        Task<int> GetTotalClickCountAsync();
    }
} 