using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ComplaintDTOs;

namespace BLL.Service
{
    public class ComplaintMessageService : IComplaintMessageService
    {
        private readonly IComplaintMessageRepository _complaintMessageRepository;
        private readonly IComplaintRepository _complaintRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ComplaintMessageService(
            IComplaintMessageRepository complaintMessageRepository,
            IComplaintRepository complaintRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _complaintMessageRepository = complaintMessageRepository;
            _complaintRepository = complaintRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<ComplaintMessageDTO>> GetComplaintMessagesAsync(int complaintId)
        {
            var messages = await _complaintMessageRepository.GetMessageCountByComplaintAsync(complaintId);
            return _mapper.Map<List<ComplaintMessageDTO>>(messages);
        }

        public async Task<ComplaintMessageDTO> AddComplaintMessageAsync(int complaintId, string userId, CreateComplaintMessageDTO createMessageDto, bool isFromAdmin)
        {
            // Validate complaint exists
            var complaint = await _complaintRepository.GetByIdAsync(complaintId);
            if (complaint == null)
                return null;

            // Validate user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("المستخدم غير موجود");

            // Create message
            var message = new ComplaintMessage
            {
                ComplaintId = complaintId,
                UserId = userId,
                Message = createMessageDto.Message,
                CreatedAt = DateTime.UtcNow,
                IsAdmin = isFromAdmin
            };

            var createdMessage = await _complaintMessageRepository.AddAsync(message);

            // Update complaint status if admin responds
            if (isFromAdmin && complaint.Status == ComplaintStatus.Pending)
            {
                complaint.Status = ComplaintStatus.InProgress;
                await _complaintRepository.UpdateAsync(complaint);
            }

            return _mapper.Map<ComplaintMessageDTO>(createdMessage);
        }
    }
} 