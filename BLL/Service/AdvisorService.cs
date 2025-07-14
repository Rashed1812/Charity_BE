using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ServiceAbstraction;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.AdvisorDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DAL.Data.Models;
<<<<<<< Updated upstream
using DAL.Repositories.RepositoryClasses;
using BLL.Services.FileService;
=======
>>>>>>> Stashed changes

namespace BLL.Service
{
    public class AdvisorService : IAdvisorService
    {
        private readonly IAdvisorRepository _advisorRepository;
        private readonly IAdvisorAvailabilityRepository _availabilityRepository;
        private readonly IAdviceRequestRepository _adviceRequestRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdvisorService(
            IAdvisorRepository advisorRepository,
            IAdvisorAvailabilityRepository availabilityRepository,
            IAdviceRequestRepository adviceRequestRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _advisorRepository = advisorRepository;
            _availabilityRepository = availabilityRepository;
            _adviceRequestRepository = adviceRequestRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<AdvisorDTO>> GetAllAdvisorsAsync()
        {
            var advisors = await _advisorRepository.GetAllAsync();
            return _mapper.Map<List<AdvisorDTO>>(advisors);
        }
        public async Task<List<AdvisorDTO>> GetAllAdvisorsWithAvailabilityAsync()
        {
            var advisors = await _advisorRepository.GetAllAdvisorsWithRelatedDataAsync();
            return _mapper.Map<List<AdvisorDTO>>(advisors);
        }

        public async Task<AdvisorDTO> GetAdvisorByIdAsync(int id)
        {
            var advisor = await _advisorRepository.GetByIdAsync(id);
            return _mapper.Map<AdvisorDTO>(advisor);
        }

        public async Task<AdvisorDTO> GetAdvisorByUserIdAsync(string userId)
        {
            var advisor = await _advisorRepository.GetAdvisorsByUserIdAsync(userId);
            return _mapper.Map<AdvisorDTO>(advisor);
        }

        public async Task<List<AdvisorDTO>> GetAdvisorsByConsultationAsync(int consultationId)
        {
            var advisors = await _advisorRepository.GetAdvisorsByConsultationAsync(consultationId);
            return _mapper.Map<List<AdvisorDTO>>(advisors);
        }

        public async Task<AdvisorDTO> CreateAdvisorAsync(CreateAdvisorDTO createAdvisorDto)
        {
            // Check if email is unique
            var existingUser = await _userManager.FindByEmailAsync(createAdvisorDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already exists");

            // Create user first
            var user = new ApplicationUser
            {
                UserName = createAdvisorDto.Email,
                Email = createAdvisorDto.Email,
                PhoneNumber = createAdvisorDto.PhoneNumber,
                FullName = createAdvisorDto.FullName,
                IsActive = true
            };

            var userResult = await _userManager.CreateAsync(user, createAdvisorDto.Password);
            if (!userResult.Succeeded)
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");

            // Add to Advisor role
            await _userManager.AddToRoleAsync(user, "Advisor");
            FileService file = new FileService();
            var imageUrl = await file.UploadFileAsync(createAdvisorDto.Image, "AdvisorImage");
            // Create advisor
            var advisor = new Advisor
            {
                UserId = user.Id,
                FullName = createAdvisorDto.FullName,
                Specialty = createAdvisorDto.Specialty,
                Description = createAdvisorDto.Description,
                ZoomRoomUrl = createAdvisorDto.ZoomRoomUrl,
                PhoneNumber = createAdvisorDto.PhoneNumber,
                Email = createAdvisorDto.Email,
                ImageUrl= imageUrl,
                ConsultationId = createAdvisorDto.ConsultationId,
                IsActive = true
            };

            var createdAdvisor = await _advisorRepository.AddAsync(advisor);
            return _mapper.Map<AdvisorDTO>(createdAdvisor);
        }

        public async Task<AdvisorDTO> UpdateAdvisorAsync(int id, UpdateAdvisorDTO updateAdvisorDto)
        {
            var advisor = await _advisorRepository.GetByIdAsync(id);
            if (advisor == null)
                return null;

            // Update advisor properties
            if (!string.IsNullOrEmpty(updateAdvisorDto.FullName))
                advisor.FullName = updateAdvisorDto.FullName;

            if (!string.IsNullOrEmpty(updateAdvisorDto.Specialty))
                advisor.Specialty = updateAdvisorDto.Specialty;

            if (!string.IsNullOrEmpty(updateAdvisorDto.Description))
                advisor.Description = updateAdvisorDto.Description;

            if (!string.IsNullOrEmpty(updateAdvisorDto.ZoomRoomUrl))
                advisor.ZoomRoomUrl = updateAdvisorDto.ZoomRoomUrl;

            if (!string.IsNullOrEmpty(updateAdvisorDto.PhoneNumber))
                advisor.PhoneNumber = updateAdvisorDto.PhoneNumber;

            if (!string.IsNullOrEmpty(updateAdvisorDto.Email))
                advisor.Email = updateAdvisorDto.Email;

            if (updateAdvisorDto.ConsultationId.HasValue)
                advisor.ConsultationId = updateAdvisorDto.ConsultationId;

            if (updateAdvisorDto.IsActive.HasValue)
                advisor.IsActive = updateAdvisorDto.IsActive.Value;

            if(updateAdvisorDto.Image !=null)
            {
                FileService fileService = new FileService();
                fileService.DeleteFile(advisor.ImageUrl!);
                var imageUrl = await fileService.UploadFileAsync(updateAdvisorDto.Image, "AdvisorImage");
                advisor.ImageUrl = imageUrl;
            }

            advisor.UpdatedAt = DateTime.UtcNow;

            var updatedAdvisor = await _advisorRepository.UpdateAsync(advisor);
            return _mapper.Map<AdvisorDTO>(updatedAdvisor);
        }

        public async Task<bool> DeleteAdvisorAsync(int id)
        {
            var advisor = await _advisorRepository.GetByIdAsync(id);
            if (advisor == null)
                return false;

            // Delete user
            var user = await _userManager.FindByIdAsync(advisor.UserId);
            if (user != null)
                await _userManager.DeleteAsync(user);

            // Delete advisor
            await _advisorRepository.DeleteAsync(advisor);
            return true;
        }

        public async Task<List<AdvisorAvailabilityDTO>> GetAdvisorAvailabilityAsync(int advisorId)
        {
            var availabilities = await _availabilityRepository.GetByAdvisorIdAsync(advisorId);
            return _mapper.Map<List<AdvisorAvailabilityDTO>>(availabilities);
        }

        public async Task<AdvisorAvailabilityDTO> CreateAvailabilityAsync(CreateAvailabilityDTO createAvailabilityDto)
        {
            // تحقق من عدم وجود موعد بنفس الوقت لنفس المستشار
            var existing = await _availabilityRepository.GetByAdvisorIdAsync(createAvailabilityDto.AdvisorId);
            if (existing.Any(a => a.Date.Date == createAvailabilityDto.Date.Date && a.Time == createAvailabilityDto.Time))
            {
                throw new InvalidOperationException("لا يمكن إضافة موعد متكرر لنفس المستشار في نفس اليوم والساعة.");
            }
            var availability = _mapper.Map<AdvisorAvailability>(createAvailabilityDto);
            availability.CreatedAt = DateTime.UtcNow;

            var createdAvailability = await _availabilityRepository.AddAsync(availability);
            return _mapper.Map<AdvisorAvailabilityDTO>(createdAvailability);
        }

        public async Task<List<AdvisorAvailabilityDTO>> CreateBulkAvailabilityAsync(BulkAvailabilityDTO bulkAvailabilityDto)
        {
            var createdAvailabilities = new List<AdvisorAvailabilityDTO>();
            
            foreach (var availabilityDto in bulkAvailabilityDto.Availabilities)
            {
                var availability = _mapper.Map<AdvisorAvailability>(availabilityDto);
                availability.AdvisorId = bulkAvailabilityDto.AdvisorId;
                availability.CreatedAt = DateTime.UtcNow;
                
                var createdAvailability = await _availabilityRepository.AddAsync(availability);
                createdAvailabilities.Add(_mapper.Map<AdvisorAvailabilityDTO>(createdAvailability));
            }

            return createdAvailabilities;
        }

        public async Task<bool> DeleteAvailabilityAsync(int id)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                return false;

            await _availabilityRepository.DeleteAsync(availability);
            return true;
        }

        public async Task<List<AdvisorRequestDTO>> GetAdvisorRequestsAsync(int advisorId)
        {
            var requests = await _adviceRequestRepository.GetByAdvisorIdAsync(advisorId);
            return _mapper.Map<List<AdvisorRequestDTO>>(requests);
        }

        public async Task<AdvisorRequestDTO> UpdateRequestStatusAsync(int requestId, ConsultationStatus status)
        {
            var request = await _adviceRequestRepository.GetByIdAsync(requestId);
            if (request == null)
                return null;

            // Validate status transition
            if (!IsValidStatusTransition(request.Status, status))
                throw new InvalidOperationException($"Invalid status transition from {request.Status} to {status}");

            request.Status = status;
            
            if (status == ConsultationStatus.Confirmed)
                request.ConfirmedDate = DateTime.UtcNow;
            else if (status == ConsultationStatus.Completed)
                request.CompletedDate = DateTime.UtcNow;

            var updatedRequest = await _adviceRequestRepository.UpdateAsync(request);
            return _mapper.Map<AdvisorRequestDTO>(updatedRequest);
        }

        public async Task<object> GetAdvisorStatisticsAsync(int advisorId)
        {
            var advisor = await _advisorRepository.GetByIdAsync(advisorId);
            if (advisor == null)
                return null;

            var totalConsultations = await _adviceRequestRepository.GetTotalConsultationsByAdvisorAsync(advisorId);
            var pendingRequests = await _adviceRequestRepository.GetPendingRequestsByAdvisorAsync(advisorId);
            var completedConsultations = await _adviceRequestRepository.GetCompletedConsultationsByAdvisorAsync(advisorId);
            var totalAvailability = await _availabilityRepository.GetTotalAvailabilityByAdvisorAsync(advisorId);
            var bookedAvailability = await _availabilityRepository.GetBookedAvailabilityByAdvisorAsync(advisorId);

            // Calculate average rating (if rating system is implemented)
            var averageRating = 0.0; // Placeholder for rating calculation

            return new
            {
                AdvisorId = advisorId,
                AdvisorName = advisor.FullName,
                TotalConsultations = totalConsultations,
                PendingRequests = pendingRequests,
                CompletedConsultations = completedConsultations,
                TotalAvailability = totalAvailability,
                BookedAvailability = bookedAvailability,
                UtilizationRate = totalAvailability > 0 ? (bookedAvailability * 100.0 / totalAvailability) : 0,
                AverageRating = averageRating
            };
        }

        private bool IsValidStatusTransition(ConsultationStatus currentStatus, ConsultationStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (ConsultationStatus.Pending, ConsultationStatus.Confirmed) => true,
                (ConsultationStatus.Pending, ConsultationStatus.Cancelled) => true,
                (ConsultationStatus.Confirmed, ConsultationStatus.Completed) => true,
                (ConsultationStatus.Confirmed, ConsultationStatus.Cancelled) => true,
                (ConsultationStatus.Confirmed, ConsultationStatus.InProgress) => true,
                (ConsultationStatus.InProgress, ConsultationStatus.Completed) => true,
                _ => false
            };
        }
    }
}
