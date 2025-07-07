using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.ServiceOfferingDTOs;

namespace BLL.Service
{
    public class ServiceOfferingService : IServiceOfferingService
    {
        private readonly IServiceOfferingRepository _serviceOfferingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ServiceOfferingService(
            IServiceOfferingRepository serviceOfferingRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _serviceOfferingRepository = serviceOfferingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceOfferingDTO>> GetAllServicesAsync()
        {
            var services = await _serviceOfferingRepository.GetAllAsync();
            return _mapper.Map<List<ServiceOfferingDTO>>(services);
        }

        public async Task<List<ServiceOfferingDTO>> GetActiveServicesAsync()
        {
            var services = await _serviceOfferingRepository.GetActiveServicesAsync();
            return _mapper.Map<List<ServiceOfferingDTO>>(services);
        }

        public async Task<ServiceOfferingDTO> GetServiceByIdAsync(int id)
        {
            var service = await _serviceOfferingRepository.GetByIdAsync(id);
            return _mapper.Map<ServiceOfferingDTO>(service);
        }

        public async Task<ServiceOfferingDTO> CreateServiceAsync(CreateServiceOfferingDTO createServiceDto)
        {
            var service = _mapper.Map<ServiceOffering>(createServiceDto);
            service.CreatedAt = DateTime.UtcNow;

            var createdService = await _serviceOfferingRepository.AddAsync(service);
            return _mapper.Map<ServiceOfferingDTO>(createdService);
        }

        public async Task<ServiceOfferingDTO> UpdateServiceAsync(int id, UpdateServiceOfferingDTO updateServiceDto)
        {
            var service = await _serviceOfferingRepository.GetByIdAsync(id);
            if (service == null)
                return null;

            // Update properties
            if (!string.IsNullOrEmpty(updateServiceDto.Name))
                service.Name = updateServiceDto.Name;

            if (!string.IsNullOrEmpty(updateServiceDto.Description))
                service.Description = updateServiceDto.Description;

            if (!string.IsNullOrEmpty(updateServiceDto.Category))
                service.Category = updateServiceDto.Category;

            if (!string.IsNullOrEmpty(updateServiceDto.ImageUrl))
                service.ImageUrl = updateServiceDto.ImageUrl;

            if (updateServiceDto.IsActive.HasValue)
                service.IsActive = updateServiceDto.IsActive.Value;

            if (!string.IsNullOrEmpty(updateServiceDto.ContactInfo))
                service.ContactInfo = updateServiceDto.ContactInfo;

            if (!string.IsNullOrEmpty(updateServiceDto.Requirements))
                service.Requirements = updateServiceDto.Requirements;

            if (!string.IsNullOrEmpty(updateServiceDto.Duration))
                service.Duration = updateServiceDto.Duration;

            if (!string.IsNullOrEmpty(updateServiceDto.Cost))
                service.Cost = updateServiceDto.Cost;

            service.UpdatedAt = DateTime.UtcNow;

            var updatedService = await _serviceOfferingRepository.UpdateAsync(service);
            return _mapper.Map<ServiceOfferingDTO>(updatedService);
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            var service = await _serviceOfferingRepository.GetByIdAsync(id);
            if (service == null)
                return false;

            await _serviceOfferingRepository.DeleteAsync(service);
            return true;
        }

        public async Task<bool> IncrementClickCountAsync(int id)
        {
            var service = await _serviceOfferingRepository.GetByIdAsync(id);
            if (service == null)
                return false;

            service.ClickCount++;
            await _serviceOfferingRepository.UpdateAsync(service);
            return true;
        }

        public async Task<object> GetServiceStatisticsAsync()
        {
            var services = await _serviceOfferingRepository.GetAllAsync();
            
            return new
            {
                TotalServices = services.Count(),
                ActiveServices = services.Count(s => s.IsActive),
                TotalClicks = services.Sum(s => s.ClickCount),
                MostClickedServices = await _serviceOfferingRepository.GetMostClickedServicesAsync(5)
            };
        }

        public async Task<List<ServiceOfferingDTO>> GetServicesByProviderAsync(string providerId)
        {
            var services = await _serviceOfferingRepository.GetServicesByProviderAsync(providerId);
            return _mapper.Map<List<ServiceOfferingDTO>>(services);
        }

        public async Task<List<ServiceOfferingDTO>> GetServicesByCategoryAsync(string category)
        {
            var services = await _serviceOfferingRepository.GetServicesByCategoryAsync(category);
            return _mapper.Map<List<ServiceOfferingDTO>>(services);
        }

        public async Task<List<ServiceOfferingDTO>> SearchServicesAsync(string searchTerm)
        {
            var services = await _serviceOfferingRepository.SearchServicesAsync(searchTerm);
            return _mapper.Map<List<ServiceOfferingDTO>>(services);
        }

        public async Task<List<ServiceOfferingDTO>> GetServicesByLocationAsync(string location)
        {
            var services = await _serviceOfferingRepository.GetServicesByLocationAsync(location);
            return _mapper.Map<List<ServiceOfferingDTO>>(services);
        }

        public async Task<List<string>> GetServiceCategoriesAsync()
        {
            return await _serviceOfferingRepository.GetServiceCategoriesAsync();
        }

        public async Task<List<string>> GetServiceLocationsAsync()
        {
            return await _serviceOfferingRepository.GetServiceLocationsAsync();
        }
    }
} 