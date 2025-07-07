using Shared.DTOS.ComplaintDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IComplaintService
    {
        // Complaint Management
        Task<List<ComplaintDTO>> GetAllComplaintsAsync();
        Task<List<ComplaintDTO>> GetUserComplaintsAsync(string userId);
        Task<ComplaintWithMessagesDTO> GetComplaintByIdAsync(int id);
        Task<ComplaintDTO> CreateComplaintAsync(string userId, CreateComplaintDTO createComplaintDto);
        Task<ComplaintDTO> UpdateComplaintAsync(int id, UpdateComplaintDTO updateComplaintDto);
        Task<bool> DeleteComplaintAsync(int id);
        Task<ComplaintDTO> UpdateComplaintStatusAsync(int id, ComplaintStatus status);

        // Message Management
        Task<List<ComplaintMessageDTO>> GetComplaintMessagesAsync(int id);
        Task<ComplaintMessageDTO> AddComplaintMessageAsync(int id, string userId, CreateComplaintMessageDTO createMessageDto, bool isAdmin);

        // Statistics
        Task<object> GetComplaintStatisticsAsync();
    }
} 