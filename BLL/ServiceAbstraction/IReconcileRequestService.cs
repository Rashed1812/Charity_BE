using Shared.DTOS.ReconcileRequestDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface IReconcileRequestService
    {
        Task<List<ReconcileRequestDTO>> GetAllAsync();
        Task<List<ReconcileRequestDTO>> GetByUserIdAsync(string userId);
        Task<ReconcileRequestDTO> GetByIdAsync(int id);
        Task<ReconcileRequestDTO> CreateAsync(string userId, CreateReconcileRequestDTO dto);
        Task<bool> DeleteAsync(int id, string userId);
    }
} 