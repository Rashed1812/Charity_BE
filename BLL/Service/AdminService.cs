using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Shared.DTOS.AdminDTOs;

namespace BLL.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, 
            UserManager<ApplicationUser> userManager)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _userManager = userManager;
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
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(createAdminDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            // Create ApplicationUser
            var user = new ApplicationUser
            {
                UserName = createAdminDto.Email,
                Email = createAdminDto.Email,
                FullName = createAdminDto.FullName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, createAdminDto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // Add to Admin role
            await _userManager.AddToRoleAsync(user, "Admin");

            // Create Admin record
            var admin = new Admin
            {
                UserId = user.Id,
                FullName = createAdminDto.FullName,
                Email = createAdminDto.Email,
                PhoneNumber = createAdminDto.PhoneNumber,
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

            _mapper.Map(updateAdminDto, admin);
            var updatedAdmin = await _adminRepository.UpdateAsync(admin);
            return _mapper.Map<AdminDTO>(updatedAdmin);
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
                return false;

            await _adminRepository.DeleteAsync(admin);
            return true;
        }

        // Additional methods implementation
        public async Task<object> GetSystemStatisticsAsync()
        {
            // TODO: Implement system statistics logic
            // This should return statistics like total users, total requests, etc.
            return new
            {
                TotalUsers = 0,
                TotalRequests = 0,
                TotalTrips = 0,
                ActiveDrivers = 0,
                ActiveNurses = 0
            };
        }

        public async Task<object> GetDashboardDataAsync()
        {
            // TODO: Implement dashboard data logic
            // This should return data for admin dashboard
            return new
            {
                RecentRequests = new List<object>(),
                RecentTrips = new List<object>(),
                SystemAlerts = new List<object>(),
                QuickStats = new
                {
                    PendingRequests = 0,
                    ActiveTrips = 0,
                    CompletedTrips = 0
                }
            };
        }

        public async Task<bool> ApproveUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            // TODO: Implement user approval logic
            // This might involve updating user status, sending notifications, etc.
            return true;
        }

        public async Task<bool> RejectUserAsync(string userId, string reason)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            // TODO: Implement user rejection logic
            // This might involve updating user status, sending notifications, etc.
            return true;
        }

        public async Task<bool> SuspendUserAsync(string userId, string reason, DateTime? until)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            // TODO: Implement user suspension logic
            // This might involve updating user status, setting suspension period, etc.
            return true;
        }

        public async Task<bool> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            // TODO: Implement user activation logic
            // This might involve updating user status, sending notifications, etc.
            return true;
        }

        public async Task<List<object>> GetSystemLogsAsync(DateTime? fromDate, DateTime? toDate, string logLevel)
        {
            // TODO: Implement system logs retrieval logic
            // This should return system logs based on the provided filters
            return new List<object>();
        }

        public async Task<bool> SendSystemNotificationAsync(string title, string message, List<string> userIds)
        {
            // TODO: Implement system notification logic
            // This should send notifications to the specified users
            return true;
        }
    }
} 