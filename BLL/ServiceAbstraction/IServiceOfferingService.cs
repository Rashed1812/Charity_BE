using Shared.DTOS.ServiceOfferingDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IServiceOfferingService
    {
        Task<List<ServiceOfferingDTO>> GetAllServicesAsync();
        Task<List<ServiceOfferingDTO>> GetActiveServicesAsync();
        Task<ServiceOfferingDTO> GetServiceByIdAsync(int id);
        Task<ServiceOfferingDTO> CreateServiceAsync(CreateServiceOfferingDTO createServiceDto);
        Task<ServiceOfferingDTO> UpdateServiceAsync(int id, UpdateServiceOfferingDTO updateServiceDto);
        Task<bool> DeleteServiceAsync(int id);
        Task<bool> IncrementClickCountAsync(int id);
    }
} 