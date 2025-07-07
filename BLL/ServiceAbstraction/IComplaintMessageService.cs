using Shared.DTOS.ComplaintDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IComplaintMessageService
    {
        Task<List<ComplaintMessageDTO>> GetComplaintMessagesAsync(int complaintId);
        Task<ComplaintMessageDTO> AddComplaintMessageAsync(int complaintId, string userId, CreateComplaintMessageDTO createMessageDto, bool isFromAdmin);
    }
} 