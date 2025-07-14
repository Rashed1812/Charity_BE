using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.AdminDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdviceRequestRepository _adviceRequestRepository;
        private readonly IComplaintRepository _complaintRepository;
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IServiceOfferingRepository _serviceOfferingRepository;
        private readonly ILectureRepository _lectureRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdminService(
            IAdminRepository adminRepository,
            IUserRepository userRepository,
            IAdviceRequestRepository adviceRequestRepository,
            IComplaintRepository complaintRepository,
            IVolunteerRepository volunteerRepository,
            IServiceOfferingRepository serviceOfferingRepository,
            ILectureRepository lectureRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _adviceRequestRepository = adviceRequestRepository;
            _complaintRepository = complaintRepository;
            _volunteerRepository = volunteerRepository;
            _serviceOfferingRepository = serviceOfferingRepository;
            _lectureRepository = lectureRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<AdminDTO>> GetAllAdminsAsync()
        {
            var admins = await _adminRepository.GetAllAsync();
            return _mapper.Map<List<AdminDTO>>(admins);
        }

        public async Task<AdminDTO> GetAdminByIdAsync(int id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            return _mapper.Map<AdminDTO>(admin);
        }

        public async Task<AdminDTO> GetAdminByUserIdAsync(string userId)
        {
            var admin = await _adminRepository.GetByUserIdAsync(userId);
            return _mapper.Map<AdminDTO>(admin);
        }

        public async Task<AdminDTO> CreateAdminAsync(CreateAdminDTO createAdminDto)
        {
            // Check if email is unique
            if (!await _adminRepository.IsEmailUniqueAsync(createAdminDto.Email))
                throw new InvalidOperationException("Email already exists");

            // Check if phone is unique
            if (!await _adminRepository.IsPhoneUniqueAsync(createAdminDto.PhoneNumber))
                throw new InvalidOperationException("Phone number already exists");

            // Create user first
            var user = new ApplicationUser
            {
                UserName = createAdminDto.Email,
                Email = createAdminDto.Email,
                PhoneNumber = createAdminDto.PhoneNumber,
                FullName = createAdminDto.FullName,
                IsActive = true
            };

            var userResult = await _userManager.CreateAsync(user, createAdminDto.Password);
            if (!userResult.Succeeded)
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");

            // Add to Admin role
            await _userManager.AddToRoleAsync(user, "Admin");

            // Create admin
            var admin = new Admin
            {
                UserId = user.Id,
                FullName = createAdminDto.FullName,
                Email = createAdminDto.Email,
                PhoneNumber = createAdminDto.PhoneNumber,
                Department = createAdminDto.Department,
                Position = createAdminDto.Position,
                IsActive = true
            };

            var createdAdmin = await _adminRepository.AddAsync(admin);
            return _mapper.Map<AdminDTO>(createdAdmin);
        }

        public async Task<AdminDTO> UpdateAdminAsync(int id, UpdateAdminDTO updateAdminDto)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
                return null;

            // Update admin properties
            if (!string.IsNullOrEmpty(updateAdminDto.FullName))
                admin.FullName = updateAdminDto.FullName;

            if (!string.IsNullOrEmpty(updateAdminDto.Email))
                admin.Email = updateAdminDto.Email;

            if (!string.IsNullOrEmpty(updateAdminDto.PhoneNumber))
                admin.PhoneNumber = updateAdminDto.PhoneNumber;

            if (!string.IsNullOrEmpty(updateAdminDto.Department))
                admin.Department = updateAdminDto.Department;

            if (!string.IsNullOrEmpty(updateAdminDto.Position))
                admin.Position = updateAdminDto.Position;

            if (updateAdminDto.IsActive.HasValue)
                admin.IsActive = updateAdminDto.IsActive.Value;

            admin.UpdatedAt = DateTime.UtcNow;

            var updatedAdmin = await _adminRepository.UpdateAsync(admin);
            return _mapper.Map<AdminDTO>(updatedAdmin);
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
                return false;

            // Delete user
            var user = await _userManager.FindByIdAsync(admin.UserId);
            if (user != null)
                await _userManager.DeleteAsync(user);

            // Delete admin
            await _adminRepository.DeleteAsync(admin);
            return true;
        }

        public async Task<object> GetSystemStatisticsAsync()
        {
            // This would typically include various system statistics
            var allAdmins = await _adminRepository.GetAllAsync();
            return new
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalAdmins = allAdmins.Count(),
                ActiveUsers = await _userManager.Users.CountAsync(u => u.IsActive),
                // Add more statistics as needed
            };
        }

        public async Task<object> GetDashboardDataAsync()
        {
            // This would typically include dashboard-specific data
            return new
            {
                RecentUsers = await _userManager.Users
                    .Where(u => u.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                    .CountAsync(),
                // Add more dashboard data as needed
            };
        }

        public async Task<bool> ApproveUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> RejectUserAsync(string userId, string reason)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> SuspendUserAsync(string userId, string reason, DateTime? until)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<List<object>> GetSystemLogsAsync(DateTime? fromDate, DateTime? toDate, string logLevel)
        {
            // This would typically query a logging system
            return new List<object>();
        }

        public async Task<bool> SendSystemNotificationAsync(string title, string message, List<string> userIds)
        {
            // This would typically send notifications to users
            return true;
        }
    }
} 