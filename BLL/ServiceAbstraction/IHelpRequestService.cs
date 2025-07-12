using Shared.DTOS.HelpDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface IHelpRequestService
    {
        Task<List<HelpRequestDTO>> GetAllAsync();
        Task<HelpRequestDTO> CreateAsync(CreateHelpRequestDTO dto);
    }
} 