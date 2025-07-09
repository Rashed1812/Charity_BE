using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.AdvisorDTOs;
using Shared.DTOS.ConsultationDTOs;
using AutoMapper;
using Shared.DTOS.NotificationDTOs;

namespace BLL.Service
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IAdviceRequestRepository _adviceRequestRepository;
        private readonly IAdvisorRepository _advisorRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public ConsultationService(
            IConsultationRepository consultationRepository,
            IAdviceRequestRepository adviceRequestRepository,
            IAdvisorRepository advisorRepository,
            IMapper mapper,
            INotificationService notificationService,
            IEmailService emailService)
        {
            _consultationRepository = consultationRepository;
            _adviceRequestRepository = adviceRequestRepository;
            _advisorRepository = advisorRepository;
            _mapper = mapper;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<IEnumerable<ConsultationDTO>> GetAllConsultationsAsync()
        {
            var consultations = await _consultationRepository.GetAllAsync();
            var consultationDtos = _mapper.Map<List<ConsultationDTO>>(consultations);

            // Add counts
            //foreach (var dto in consultationDtos)
            //{
            //    dto.AdvisorCount = await _consultationRepository.GetAdvisorCountAsync(dto.Id);
            //    dto.RequestCount = await _consultationRepository.GetRequestCountAsync(dto.Id);
            //    dto.LectureCount = await _consultationRepository.GetLectureCountAsync(dto.Id);
            //}

            return consultationDtos;
        }

        public async Task<List<ConsultationDTO>> GetActiveConsultationsAsync()
        {
            var consultations = await _consultationRepository.GetActiveConsultationsAsync();
            var consultationDtos = _mapper.Map<List<ConsultationDTO>>(consultations);

            // Add counts
            foreach (var dto in consultationDtos)
            {
                dto.AdvisorCount = await _consultationRepository.GetAdvisorCountAsync(dto.Id);
                dto.RequestCount = await _consultationRepository.GetRequestCountAsync(dto.Id);
                dto.LectureCount = await _consultationRepository.GetLectureCountAsync(dto.Id);
            }

            return consultationDtos;
        }

        public async Task<ConsultationDTO> GetConsultationByIdAsync(int id)
        {
            var consultation = await _consultationRepository.GetByIdAsync(id);
            if (consultation == null)
                return null;

            var dto = _mapper.Map<ConsultationDTO>(consultation);
            dto.AdvisorCount = await _consultationRepository.GetAdvisorCountAsync(id);
            dto.RequestCount = await _consultationRepository.GetRequestCountAsync(id);
            dto.LectureCount = await _consultationRepository.GetLectureCountAsync(id);

            return dto;
        }

        public async Task<ConsultationDTO> CreateConsultationAsync(CreateConsultationDTO createConsultationDto)
        {
            // Check if name already exists
            var existingConsultation = await _consultationRepository.GetByNameAsync(createConsultationDto.ConsultationName);
            if (existingConsultation != null)
                throw new InvalidOperationException("Consultation name already exists");

            var consultation = _mapper.Map<Consultation>(createConsultationDto);
            consultation.CreatedAt = DateTime.UtcNow;

            var createdConsultation = await _consultationRepository.AddAsync(consultation);

            // إرسال إشعار للأدمن عند إضافة استشارة جديدة
            var adminEmail = "admin@example.com"; // عدل هذا لاحقًا لجلب كل الأدمنز
            var notification = new NotificationCreateDTO {
                UserId = "admin", // عدل هذا لاحقًا ليكون لكل الأدمنز
                Title = "استشارة جديدة",
                Message = $"تم تقديم طلب استشارة جديدة بعنوان: {consultation.ConsultationName}",
                Type = NotificationType.Consultation
            };
            await _notificationService.AddNotificationAsync(notification);
            await _emailService.SendEmailAsync(adminEmail, notification.Title, notification.Message);

            return _mapper.Map<ConsultationDTO>(createdConsultation);
        }

        public async Task<ConsultationDTO> UpdateConsultationAsync(ConsultationDTO consultationDto)
        {
            var consultation = await _consultationRepository.GetByIdAsync(consultationDto.Id);
            if (consultation == null)
                return null;

            // Check if name already exists (if changing name)
            if (!string.IsNullOrEmpty(consultationDto.ConsultationName) && 
                consultationDto.ConsultationName != consultation.ConsultationName)
            {
                var existingConsultation = await _consultationRepository.GetByNameAsync(consultationDto.ConsultationName);
                if (existingConsultation != null)
                    throw new InvalidOperationException("Consultation name already exists");
            }

            // Update properties
            if (!string.IsNullOrEmpty(consultationDto.ConsultationName))
                consultation.ConsultationName = consultationDto.ConsultationName;

            if (!string.IsNullOrEmpty(consultationDto.Description))
                consultation.Description = consultationDto.Description;

            consultation.IsActive = consultationDto.IsActive;
            consultation.UpdatedAt = DateTime.UtcNow;

            var updatedConsultation = await _consultationRepository.UpdateAsync(consultation);
            return _mapper.Map<ConsultationDTO>(updatedConsultation);
        }

        public async Task<bool> DeleteConsultationAsync(int id)
        {
            var consultation = await _consultationRepository.GetByIdAsync(id);
            if (consultation == null)
                return false;

            await _consultationRepository.DeleteAsync(consultation);
            return true;
        }

        public async Task<ConsultationDTO> ToggleConsultationStatusAsync(int id)
        {
            var consultation = await _consultationRepository.GetByIdAsync(id);
            if (consultation == null)
                return null;

            // Toggle status
            consultation.IsActive = !consultation.IsActive;
            consultation.UpdatedAt = DateTime.UtcNow;

            var updatedConsultation = await _consultationRepository.UpdateAsync(consultation);
            return _mapper.Map<ConsultationDTO>(updatedConsultation);
        }

        public async Task<object> GetConsultationStatisticsAsync(int consultationId)
        {
            var consultation = await _consultationRepository.GetByIdAsync(consultationId);
            if (consultation == null)
                return null;

            var totalRequests = await _adviceRequestRepository.GetRequestsCountByConsultationAsync(consultationId);
            var pendingRequests = await _adviceRequestRepository.GetPendingRequestsCountByConsultationAsync(consultationId);
            var completedRequests = await _adviceRequestRepository.GetCompletedRequestsCountByConsultationAsync(consultationId);
            var totalAdvisors = await _advisorRepository.GetAdvisorsCountByConsultationAsync(consultationId);
            var activeAdvisors = await _advisorRepository.GetActiveAdvisorsCountByConsultationAsync(consultationId);

            // Get requests by month for the last 6 months
            var monthlyRequests = await _adviceRequestRepository.GetRequestsByConsultationAndMonthAsync(consultationId, 6);

            // Get top advisors for this consultation
            var topAdvisors = await _advisorRepository.GetTopAdvisorsByConsultationAsync(consultationId, 10);

            return new
            {
                ConsultationId = consultationId,
                ConsultationName = consultation.ConsultationName,
                TotalRequests = totalRequests,
                PendingRequests = pendingRequests,
                CompletedRequests = completedRequests,
                TotalAdvisors = totalAdvisors,
                ActiveAdvisors = activeAdvisors,
                CompletionRate = totalRequests > 0 ? (completedRequests * 100.0 / totalRequests) : 0,
                AdvisorUtilizationRate = totalAdvisors > 0 ? (activeAdvisors * 100.0 / totalAdvisors) : 0,
                MonthlyRequests = monthlyRequests,
                TopAdvisors = topAdvisors
            };
        }

        public async Task<object> GetAllConsultationsStatisticsAsync()
        {
            var consultations = await _consultationRepository.GetAllAsync();
            var statistics = new List<object>();

            foreach (var consultation in consultations)
            {
                var consultationStats = await GetConsultationStatisticsAsync(consultation.Id);
                if (consultationStats != null)
                {
                    statistics.Add(consultationStats);
                }
            }

            return new
            {
                TotalConsultations = consultations.Count(),
                ActiveConsultations = consultations.Count(c => c.IsActive),
                Statistics = statistics
            };
        }

        public async Task<ConsultationDTO> GetConsultationByIdWithRelatedDataAsync(int id)
        {
            var consultation = await _consultationRepository.GetByIdWithIncludesAsync(id);
            if (consultation == null)
                return null;

            var dto = _mapper.Map<ConsultationDTO>(consultation);
            dto.AdvisorCount = await _consultationRepository.GetAdvisorCountAsync(id);
            dto.RequestCount = await _consultationRepository.GetRequestCountAsync(id);
            dto.LectureCount = await _consultationRepository.GetLectureCountAsync(id);

            return dto;
        }

        public async Task<IEnumerable<ConsultationDTO>> GetAllConsultationsWithRelatedDataAsync()
        {
            var consultations = await _consultationRepository.GetAllWithIncludesAsync();
            var consultationDtos = _mapper.Map<List<ConsultationDTO>>(consultations);

            // Add counts
            foreach (var dto in consultationDtos)
            {
                dto.AdvisorCount = await _consultationRepository.GetAdvisorCountAsync(dto.Id);
                dto.RequestCount = await _consultationRepository.GetRequestCountAsync(dto.Id);
                dto.LectureCount = await _consultationRepository.GetLectureCountAsync(dto.Id);
            }

            return consultationDtos;
        }

        public async Task<object> GetConsultationStatisticsAsync()
        {
            var consultations = await _consultationRepository.GetAllAsync();
            
            var totalConsultations = consultations.Count();
            var activeConsultations = consultations.Count(c => c.IsActive);
            var totalRequests = 0;
            var totalAdvisors = 0;

            foreach (var consultation in consultations)
            {
                totalRequests += await _consultationRepository.GetRequestCountAsync(consultation.Id);
                totalAdvisors += await _consultationRepository.GetAdvisorCountAsync(consultation.Id);
            }

            return new
            {
                TotalConsultations = totalConsultations,
                ActiveConsultations = activeConsultations,
                InactiveConsultations = totalConsultations - activeConsultations,
                TotalRequests = totalRequests,
                TotalAdvisors = totalAdvisors,
                AverageRequestsPerConsultation = totalConsultations > 0 ? (double)totalRequests / totalConsultations : 0,
                AverageAdvisorsPerConsultation = totalConsultations > 0 ? (double)totalAdvisors / totalConsultations : 0
            };
        }

        public Task<ConsultationDTO> UpdateConsultationAsync(int id, UpdateConsultationDTO updateConsultationDto)
        {
            var consultation = _mapper.Map<Consultation>(updateConsultationDto);
            consultation.Id = id;
            return UpdateConsultationAsync(_mapper.Map<ConsultationDTO>(consultation));
        }
    }
}
