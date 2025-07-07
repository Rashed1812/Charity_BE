using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.UserDTO;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace BLL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdviceRequestRepository _adviceRequestRepository;
        private readonly IComplaintRepository _complaintRepository;
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IServiceOfferingRepository _serviceOfferingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IAdviceRequestRepository adviceRequestRepository,
            IComplaintRepository complaintRepository,
            IVolunteerRepository volunteerRepository,
            IServiceOfferingRepository serviceOfferingRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _adviceRequestRepository = adviceRequestRepository;
            _complaintRepository = complaintRepository;
            _volunteerRepository = volunteerRepository;
            _serviceOfferingRepository = serviceOfferingRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDTO>>(users);

            // Add roles to each user
            foreach (var dto in userDtos)
            {
                var user = users.First(u => u.Id == dto.Id);
                var roles = await _userManager.GetRolesAsync(user);
                dto.Roles = roles.ToList();
            }

            return userDtos;
        }

        public async Task<List<UserDTO>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            var dto = _mapper.Map<UserDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            dto.Roles = roles.ToList();

            return dto;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            var dto = _mapper.Map<UserDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            dto.Roles = roles.ToList();

            return dto;
        }

        public async Task<UserDTO> UpdateUserAsync(string id, UpdateUserDTO updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            // Check if email is unique (if changing email)
            if (!string.IsNullOrEmpty(updateUserDto.Email) && updateUserDto.Email != user.Email)
            {
                if (!await _userRepository.IsEmailUniqueAsync(updateUserDto.Email))
                    throw new InvalidOperationException("Email already exists");
            }

            // Check if phone is unique (if changing phone)
            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber) && updateUserDto.PhoneNumber != user.PhoneNumber)
            {
                if (!await _userRepository.IsPhoneUniqueAsync(updateUserDto.PhoneNumber))
                    throw new InvalidOperationException("Phone number already exists");
            }

            // Update properties

            if (!string.IsNullOrEmpty(updateUserDto.FullName))
                user.FullName = updateUserDto.FullName;

            if (!string.IsNullOrEmpty(updateUserDto.Email))
                user.Email = updateUserDto.Email;

            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
                user.PhoneNumber = updateUserDto.PhoneNumber;

            if (!string.IsNullOrEmpty(updateUserDto.Address))
                user.Address = updateUserDto.Address;

            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(user);
            var dto = _mapper.Map<UserDTO>(updatedUser);
            var roles = await _userManager.GetRolesAsync(updatedUser);
            dto.Roles = roles.ToList();

            return dto;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            await _userManager.DeleteAsync(user);
            return true;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<object> GetUserDashboardAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            var totalConsultations = await _adviceRequestRepository.GetTotalConsultationsByUserAsync(userId);
            var pendingConsultations = await _adviceRequestRepository.GetPendingConsultationsByUserAsync(userId);
            var completedConsultations = await _adviceRequestRepository.GetCompletedConsultationsByUserAsync(userId);
            var totalComplaints = await _complaintRepository.GetTotalComplaintsByUserAsync(userId);
            var pendingComplaints = await _complaintRepository.GetPendingComplaintsByUserAsync(userId);
            var volunteerApplications = await _volunteerRepository.GetVolunteerApplicationsByUserAsync(userId);

            return new
            {
                UserId = userId,
                UserName = user.FullName,
                TotalConsultations = totalConsultations,
                PendingConsultations = pendingConsultations,
                CompletedConsultations = completedConsultations,
                TotalComplaints = totalComplaints,
                PendingComplaints = pendingComplaints,
                VolunteerApplications = volunteerApplications.Count,
                LastLogin = user.LastLoginAt,
                IsActive = user.IsActive
            };
        }

        public async Task<object> GetUserStatisticsAsync()
        {
            var totalUsers = await _userRepository.GetTotalUsersCountAsync();
            var activeUsers = await _userRepository.GetActiveUsersCountAsync();
            var newUsersThisMonth = await _userRepository.GetNewUsersThisMonthAsync();
            var usersByRole = await _userRepository.GetUsersCountByRoleAsync();

            return new
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                NewUsersThisMonth = newUsersThisMonth,
                UsersByRole = usersByRole
            };
        }

        public async Task<List<UserDTO>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm);
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetUsersByRoleAsync(string role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<bool> UpdateUserProfilePictureAsync(string userId, string profilePictureUrl)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.ProfilePictureUrl = profilePictureUrl;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> VerifyEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ActivateUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeactivateUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
