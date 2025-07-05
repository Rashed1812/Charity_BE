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
        //Get All Advisors
        Task<IEnumerable<AdvisorDTO>> GetAllAdvisorsAsync();
        
        //GetAllWithRelatedData
        Task<IEnumerable<AdvisorDTO>> GetAllAdvisorsWithRelatedDataAsync();

        //GetAdvisorById
        Task<AdvisorDTO> GetAdvisorByIdAsync(int id);
        //GetByIdWithRelatedData
        Task<AdvisorDTO> GetAdvisorByIdWithRelatedDataAsync(int id);
        //UpdateAdvisor
        Task<AdvisorDTO> UpdateAdvisorAsync(AdvisorDTO advisorDto);

        //DeleteAdvisor
        Task<bool> DeleteAdvisorAsync(int id);
        //AdvisorsByConsultation
        Task<IEnumerable<AdvisorDTO>> GetAdvisorsByConsultationAsync(int consultationId);

        //GetAdvisorsByAvailability
        Task<IEnumerable<AdvisorDTO>> GetAdvisorsByAvailabilityAsync(int availabilityId);

    }
}
