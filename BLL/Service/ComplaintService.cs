using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.ComplaintDTOs;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.NotificationDTOs;

namespace BLL.Service
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public ComplaintService(
            IComplaintRepository complaintRepository,
            IUserRepository userRepository,
            IMapper mapper,
            INotificationService notificationService,
            IEmailService emailService)
        {
            _complaintRepository = complaintRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _notificationService = notificationService;
            _emailService = emailService;
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

        public async Task<ComplaintDTO> CreateComplaintAsync(string userId, CreateComplaintDTO createComplaintDto)
        {
            var complaint = _mapper.Map<Complaint>(createComplaintDto);
            complaint.UserId = userId;
            complaint.Status = ComplaintStatus.Pending;
            complaint.CreatedAt = DateTime.UtcNow;

            var createdComplaint = await _complaintRepository.AddAsync(complaint);

            // إرسال إشعار للأدمن عند إضافة شكوى جديدة
            var adminEmail = "admin@example.com"; // عدل هذا لاحقًا لجلب كل الأدمنز
            var notification = new NotificationCreateDTO {
                UserId = "admin", // عدل هذا لاحقًا ليكون لكل الأدمنز
                Title = "شكوى جديدة",
                Message = $"تم تقديم شكوى جديدة بعنوان: {complaint.Title}",
                Type = NotificationType.Complaint
            };
            await _notificationService.AddNotificationAsync(notification);
            await _emailService.SendEmailAsync(adminEmail, notification.Title, notification.Message);

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

            if (updateComplaintDto.Category.HasValue)
                complaint.Category = updateComplaintDto.Category.Value;

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

            // إرسال إشعار للمستخدم عند تغيير حالة الشكوى
            var user = await _userRepository.GetByIdAsync(complaint.UserId);
            if (user != null)
            {
                var notification = new NotificationCreateDTO {
                    UserId = user.Id,
                    Title = "تحديث حالة الشكوى",
                    Message = $"تم تغيير حالة الشكوى الخاصة بك إلى: {status}",
                    Type = NotificationType.Complaint
                };
                await _notificationService.AddNotificationAsync(notification);
                await _emailService.SendEmailAsync(user.Email, notification.Title, notification.Message);
            }

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