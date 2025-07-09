using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.AdvisorDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IAdvisorService
    {
        // Advisor Management
        Task<List<AdvisorDTO>> GetAllAdvisorsAsync();
        Task<AdvisorDTO> GetAdvisorByIdAsync(int id);
        Task<List<AdvisorDTO>> GetAdvisorsByConsultationAsync(int consultationId);
        Task<AdvisorDTO> CreateAdvisorAsync(CreateAdvisorDTO createAdvisorDto);
        Task<AdvisorDTO> UpdateAdvisorAsync(int id, UpdateAdvisorDTO updateAdvisorDto);
        Task<bool> DeleteAdvisorAsync(int id);

        // Availability Management
        Task<List<AdvisorDTO>> GetAllAdvisorsWithAvailabilityAsync();
        Task<List<AdvisorAvailabilityDTO>> GetAdvisorAvailabilityAsync(int advisorId);
        Task<AdvisorAvailabilityDTO> CreateAvailabilityAsync(CreateAvailabilityDTO createAvailabilityDto);
        Task<List<AdvisorAvailabilityDTO>> CreateBulkAvailabilityAsync(BulkAvailabilityDTO bulkAvailabilityDto);
        Task<bool> DeleteAvailabilityAsync(int availabilityId);

        // Consultation Requests
        Task<List<AdvisorRequestDTO>> GetAdvisorRequestsAsync(int advisorId);
        Task<AdvisorRequestDTO> UpdateRequestStatusAsync(int requestId, ConsultationStatus status);

        // Statistics
        Task<object> GetAdvisorStatisticsAsync(int advisorId);
    }
}
