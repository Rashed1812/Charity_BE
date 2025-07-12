using Shared.DTOS.HelpDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface IHelpTypeService
    {
        Task<List<HelpTypeDTO>> GetAllAsync();
        Task<HelpTypeDTO> GetByIdAsync(int id);
        Task<HelpTypeDTO> CreateAsync(CreateHelpTypeDTO dto);
        Task<HelpTypeDTO> UpdateAsync(int id, CreateHelpTypeDTO dto);
        Task<bool> DeleteAsync(int id);
    }
} 