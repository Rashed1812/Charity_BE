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
        Task<AdvisorAvailabilityDTO> UpdateAvailabilityAsync(int id, UpdateAvailabilityDTO updateAvailabilityDto);
        Task<bool> DeleteAvailabilityAsync(int id);
        Task<List<AdvisorAvailabilityDTO>> GetAvailableSlotsAsync(int advisorId, DateTime date);
        Task<List<AdvisorAvailabilityDTO>> GetAvailableSlotsByTypeAsync(int advisorId, DateTime date, ConsultationType consultationType);

        // Consultation Requests
        Task<List<AdvisorRequestDTO>> GetAdvisorRequestsAsync(int advisorId);
        Task<AdvisorRequestDTO> UpdateRequestStatusAsync(int requestId, ConsultationStatus status);

        // Statistics
        Task<object> GetAdvisorStatisticsAsync(int advisorId);
    }
}
