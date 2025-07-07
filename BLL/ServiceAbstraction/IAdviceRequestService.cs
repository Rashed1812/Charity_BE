using Shared.DTOS.AdviceRequestDTOs;
using Shared.DTOS.AdvisorDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IAdviceRequestService
    {
        Task<List<AdviceRequestDTO>> GetAllRequestsAsync();
        Task<List<AdviceRequestDTO>> GetUserRequestsAsync(string userId);
        Task<AdviceRequestDTO> GetRequestByIdAsync(int id);
        Task<AdviceRequestDTO> CreateRequestAsync(string userId, CreateAdviceRequestDTO createRequestDto);
        Task<AdviceRequestDTO> UpdateRequestAsync(int id, string userId, UpdateAdviceRequestDTO updateRequestDto);
        Task<bool> CancelRequestAsync(int id, string userId);
        Task<AdviceRequestDTO> ConfirmRequestAsync(int id, string advisorId);
        Task<AdviceRequestDTO> CompleteRequestAsync(int id, string advisorId, CompleteRequestDTO completeRequestDto);
        Task<List<AdviceRequestDTO>> GetRequestsByStatusAsync(ConsultationStatus status);
        Task<List<AdviceRequestDTO>> GetRequestsByAdvisorAsync(int advisorId);
        Task<List<AdviceRequestDTO>> GetRequestsByConsultationAsync(int consultationId);
        Task<object> GetRequestStatisticsAsync();
    }
} 