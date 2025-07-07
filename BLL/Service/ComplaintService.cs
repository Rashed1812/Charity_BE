using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.ComplaintDTOs;
using Microsoft.EntityFrameworkCore;

namespace BLL.Service
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IComplaintMessageRepository _complaintMessageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ComplaintService(
            IComplaintRepository complaintRepository,
            IComplaintMessageRepository complaintMessageRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _complaintRepository = complaintRepository;
            _complaintMessageRepository = complaintMessageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<ComplaintDTO>> GetAllComplaintsAsync()
        {
            var complaints = await _complaintRepository.GetAllAsync();
            return _mapper.Map<List<ComplaintDTO>>(complaints);
        }

        public async Task<List<ComplaintDTO>> GetUserComplaintsAsync(string userId)
        {
            var complaints = await _complaintRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<ComplaintDTO>>(complaints);
        }

        public async Task<ComplaintWithMessagesDTO> GetComplaintByIdAsync(int id)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return null;

            var messages = await _complaintMessageRepository.GetByComplaintIdAsync(id);
            var complaintDto = _mapper.Map<ComplaintWithMessagesDTO>(complaint);
            complaintDto.Messages = _mapper.Map<List<ComplaintMessageDTO>>(messages);

            return complaintDto;
        }

        public async Task<ComplaintDTO> CreateComplaintAsync(string userId, CreateComplaintDTO createComplaintDto)
        {
            var complaint = _mapper.Map<Complaint>(createComplaintDto);
            complaint.UserId = userId;
            complaint.Status = ComplaintStatus.Pending;
            complaint.CreatedAt = DateTime.UtcNow;

            var createdComplaint = await _complaintRepository.AddAsync(complaint);
            return _mapper.Map<ComplaintDTO>(createdComplaint);
        }

        public async Task<ComplaintDTO> UpdateComplaintAsync(int id, UpdateComplaintDTO updateComplaintDto)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return null;

            // Update properties
            if (!string.IsNullOrEmpty(updateComplaintDto.Title))
                complaint.Title = updateComplaintDto.Title;

            if (!string.IsNullOrEmpty(updateComplaintDto.Description))
                complaint.Description = updateComplaintDto.Description;

            if (!string.IsNullOrEmpty(updateComplaintDto.Category))
                complaint.Category = updateComplaintDto.Category;

            if (!string.IsNullOrEmpty(updateComplaintDto.Priority))
                complaint.Priority = updateComplaintDto.Priority;

            if (!string.IsNullOrEmpty(updateComplaintDto.Status))
                complaint.Status = (ComplaintStatus)Enum.Parse(typeof(ComplaintStatus), updateComplaintDto.Status);

            if (!string.IsNullOrEmpty(updateComplaintDto.Resolution))
                complaint.Resolution = updateComplaintDto.Resolution;

            complaint.UpdatedAt = DateTime.UtcNow;

            // Set resolved date if status is Resolved
            if (complaint.Status == ComplaintStatus.Resolved && !complaint.ResolvedAt.HasValue)
                complaint.ResolvedAt = DateTime.UtcNow;

            var updatedComplaint = await _complaintRepository.UpdateAsync(complaint);
            return _mapper.Map<ComplaintDTO>(updatedComplaint);
        }

        public async Task<bool> DeleteComplaintAsync(int id)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return false;

            await _complaintRepository.DeleteAsync(complaint);
            return true;
        }

        public async Task<List<ComplaintMessageDTO>> GetComplaintMessagesAsync(int id)
        {
            var messages = await _complaintMessageRepository.GetByComplaintIdAsync(id);
            return _mapper.Map<List<ComplaintMessageDTO>>(messages);
        }

        public async Task<ComplaintMessageDTO> AddComplaintMessageAsync(int id, string userId, CreateComplaintMessageDTO createMessageDto, bool isAdmin)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return null;

            var message = _mapper.Map<ComplaintMessage>(createMessageDto);
            message.ComplaintId = id;
            message.UserId = userId;
            message.IsAdmin = isAdmin;
            message.CreatedAt = DateTime.UtcNow;

            var createdMessage = await _complaintMessageRepository.AddAsync(message);
            return _mapper.Map<ComplaintMessageDTO>(createdMessage);
        }

        public async Task<ComplaintDTO> UpdateComplaintStatusAsync(int id, ComplaintStatus status)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return null;

            complaint.Status = status;
            complaint.UpdatedAt = DateTime.UtcNow;

            // Set resolved date if status is Resolved
            if (status == ComplaintStatus.Resolved && !complaint.ResolvedAt.HasValue)
                complaint.ResolvedAt = DateTime.UtcNow;

            var updatedComplaint = await _complaintRepository.UpdateAsync(complaint);
            return _mapper.Map<ComplaintDTO>(updatedComplaint);
        }

        public async Task<object> GetComplaintStatisticsAsync()
        {
            var complaints = await _complaintRepository.GetAllAsync();
            
            return new
            {
                TotalComplaints = complaints.Count(),
                PendingComplaints = complaints.Count(c => c.Status == ComplaintStatus.Pending),
                InProgressComplaints = complaints.Count(c => c.Status == ComplaintStatus.InProgress),
                ResolvedComplaints = complaints.Count(c => c.Status == ComplaintStatus.Resolved),
                ClosedComplaints = complaints.Count(c => c.Status == ComplaintStatus.Closed)
            };
        }
    }
} 