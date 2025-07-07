using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.ConsultationDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IConsultationService
    {
        //Create Consultation
        Task<ConsultationDTO> CreateConsultationAsync(CreateConsultationDTO consultationDto);
        // Get All Consultations
        Task<IEnumerable<ConsultationDTO>> GetAllConsultationsAsync();
        // Get Consultation By Id
        Task<ConsultationDTO> GetConsultationByIdAsync(int id);
        // Get Consultation By Id with related data
        Task<ConsultationDTO> GetConsultationByIdWithRelatedDataAsync(int id);
        // Update Consultation
        Task<ConsultationDTO> UpdateConsultationAsync(ConsultationDTO consultationDto);
        // Update Consultation with UpdateConsultationDTO
        Task<ConsultationDTO> UpdateConsultationAsync(int id, UpdateConsultationDTO updateConsultationDto);
        // Delete Consultation
        Task<bool> DeleteConsultationAsync(int id);

        // Get All Consultations with related data
        Task<IEnumerable<ConsultationDTO>> GetAllConsultationsWithRelatedDataAsync();

        Task<List<ConsultationDTO>> GetActiveConsultationsAsync();
        Task<object> GetConsultationStatisticsAsync();
        Task<object> GetConsultationStatisticsAsync(int consultationId);
        Task<object> GetAllConsultationsStatisticsAsync();
        Task<ConsultationDTO> ToggleConsultationStatusAsync(int id);
    }
}
