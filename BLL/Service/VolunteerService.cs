using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.VolunteerDTOs;

namespace BLL.Service
{
    public class VolunteerService : IVolunteerService
    {
        private readonly IVolunteerApplicationRepository _volunteerApplicationRepository;
        private readonly IMapper _mapper;

        public VolunteerService(IVolunteerApplicationRepository volunteerApplicationRepository, IMapper mapper)
        {
            _volunteerApplicationRepository = volunteerApplicationRepository;
            _mapper = mapper;
        }

        public async Task<List<VolunteerApplicationDTO>> GetAllApplicationsAsync()
        {
            var applications = await _volunteerApplicationRepository.GetAllAsync();
            return _mapper.Map<List<VolunteerApplicationDTO>>(applications);
        }

        public async Task<VolunteerApplicationDTO> GetUserApplicationAsync(string userId)
        {
            var application = await _volunteerApplicationRepository.GetByUserIdAsync(userId);
            return _mapper.Map<VolunteerApplicationDTO>(application);
        }

        public async Task<VolunteerApplicationDTO> GetApplicationByIdAsync(int id)
        {
            var application = await _volunteerApplicationRepository.GetByIdAsync(id);
            return _mapper.Map<VolunteerApplicationDTO>(application);
        }

        public async Task<VolunteerApplicationDTO> CreateApplicationAsync(string userId, CreateVolunteerApplicationDTO createApplicationDto)
        {
            // Check if user already has an application
            var existingApplication = await _volunteerApplicationRepository.GetByUserIdAsync(userId);
            if (existingApplication != null)
                throw new InvalidOperationException("User already has a volunteer application");

            var application = _mapper.Map<VolunteerApplication>(createApplicationDto);
            application.UserId = userId;
            application.Status = VolunteerStatus.Pending;
            application.AppliedAt = DateTime.UtcNow;

            var createdApplication = await _volunteerApplicationRepository.AddAsync(application);
            return _mapper.Map<VolunteerApplicationDTO>(createdApplication);
        }

        public async Task<VolunteerApplicationDTO> UpdateApplicationAsync(int id, string userId, UpdateVolunteerApplicationDTO updateApplicationDto)
        {
            var application = await _volunteerApplicationRepository.GetByIdAsync(id);
            if (application == null || application.UserId != userId)
                return null;

            // Only allow updates if application is still pending
            if (application.Status != VolunteerStatus.Pending)
                throw new InvalidOperationException("Cannot update application that is not pending");

            // Update properties
            if (!string.IsNullOrEmpty(updateApplicationDto.FirstName))
                application.FirstName = updateApplicationDto.FirstName;

            if (!string.IsNullOrEmpty(updateApplicationDto.LastName))
                application.LastName = updateApplicationDto.LastName;

            if (!string.IsNullOrEmpty(updateApplicationDto.Email))
                application.Email = updateApplicationDto.Email;

            if (!string.IsNullOrEmpty(updateApplicationDto.PhoneNumber))
                application.PhoneNumber = updateApplicationDto.PhoneNumber;

            if (updateApplicationDto.DateOfBirth.HasValue)
                application.DateOfBirth = updateApplicationDto.DateOfBirth.Value;

            if (!string.IsNullOrEmpty(updateApplicationDto.Address))
                application.Address = updateApplicationDto.Address;

            if (!string.IsNullOrEmpty(updateApplicationDto.City))
                application.City = updateApplicationDto.City;

            if (!string.IsNullOrEmpty(updateApplicationDto.Country))
                application.Country = updateApplicationDto.Country;

            if (!string.IsNullOrEmpty(updateApplicationDto.Education))
                application.Education = updateApplicationDto.Education;

            if (!string.IsNullOrEmpty(updateApplicationDto.Skills))
                application.Skills = updateApplicationDto.Skills;

            if (!string.IsNullOrEmpty(updateApplicationDto.Experience))
                application.Experience = updateApplicationDto.Experience;

            if (!string.IsNullOrEmpty(updateApplicationDto.Motivation))
                application.Motivation = updateApplicationDto.Motivation;

            if (!string.IsNullOrEmpty(updateApplicationDto.Availability))
                application.Availability = updateApplicationDto.Availability;

            var updatedApplication = await _volunteerApplicationRepository.UpdateAsync(application);
            return _mapper.Map<VolunteerApplicationDTO>(updatedApplication);
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _volunteerApplicationRepository.GetByIdAsync(id);
            if (application == null)
                return false;

            await _volunteerApplicationRepository.DeleteAsync(application);
            return true;
        }

        public async Task<VolunteerApplicationDTO> ReviewApplicationAsync(int id, string adminId, ReviewVolunteerApplicationDTO reviewDto)
        {
            var application = await _volunteerApplicationRepository.GetByIdAsync(id);
            if (application == null)
                return null;

            application.Status = (VolunteerStatus)Enum.Parse(typeof(VolunteerStatus), reviewDto.Status);
            application.AdminNotes = reviewDto.AdminNotes;
            application.ReviewedAt = DateTime.UtcNow;
            application.ReviewedBy = adminId;

            var updatedApplication = await _volunteerApplicationRepository.UpdateAsync(application);
            return _mapper.Map<VolunteerApplicationDTO>(updatedApplication);
        }

        public async Task<List<VolunteerApplicationDTO>> GetApplicationsByStatusAsync(VolunteerStatus status)
        {
            var applications = await _volunteerApplicationRepository.GetByStatusAsync(status);
            return _mapper.Map<List<VolunteerApplicationDTO>>(applications);
        }

        public async Task<object> GetVolunteerStatisticsAsync()
        {
            var applications = await _volunteerApplicationRepository.GetAllAsync();
            
            return new
            {
                TotalApplications = applications.Count(),
                PendingApplications = applications.Count(a => a.Status == VolunteerStatus.Pending),
                ApprovedApplications = applications.Count(a => a.Status == VolunteerStatus.Approved),
                RejectedApplications = applications.Count(a => a.Status == VolunteerStatus.Rejected),
                UnderReviewApplications = applications.Count(a => a.Status == VolunteerStatus.UnderReview)
            };
        }
    }
} 