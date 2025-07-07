using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Shared.DTOS.AuthDTO;
using BLL.ServiceAbstraction;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Data;

namespace BLL.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _adminRepository;
        private readonly IAdvisorRepository _advisorRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IConfiguration configuration,
            IAdminRepository adminRepository,
            IAdvisorRepository advisorRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
            _adminRepository = adminRepository;
            _advisorRepository = advisorRepository;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var siginCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"]!)),
                signingCredentials: siginCreds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponseDTO> RegisterAdminAsync(RegisterAdminDTO dto)
        {
            if (await IsEmailExistAsync(dto.Email))
                throw new Exception("Email already exists");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Admin");

            var admin = new Admin
            {
                UserId = user.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Department = dto.Department,
                Position = dto.Position,
                IsActive = true
            };

            await _adminRepository.AddAsync(admin);

            var token = await CreateTokenAsync(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = "Admin",
                Token = token
            };
        }

        public async Task<AuthResponseDTO> RegisterAdvisorAsync(RegisterAdvisorDTO dto)
        {
            if (await IsEmailExistAsync(dto.Email))
                throw new Exception("Email already exists");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Advisor");

            var advisor = new Advisor
            {
                UserId = user.Id,
                FullName = dto.FullName,
                Specialty = dto.Specialty,
                Description = dto.Description,
                ZoomRoomUrl = dto.ZoomRoomUrl,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                IsActive = true
            };

            await _advisorRepository.AddAsync(advisor);

            var token = await CreateTokenAsync(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = "Advisor",
                Token = token
            };
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                FullName = dto.FullName,
                Address = dto.Address,
                IsActive = true,
                ProfilePictureUrl = dto.ProfilePictureUrl,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var currentUserDto = new CurrentUserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Role = roles.ToList(),
                IsActive = user.IsActive
            };

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "User",  
                Token = token,
                Success = true,
                Message = "Registration successful",
                RefreshToken = null,  
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = currentUserDto
            };

        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var normalizedEmail = dto.Email?.Trim().ToUpperInvariant();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

            if (user == null)
                throw new Exception("Invalid credentials");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordCorrect)
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var token = await CreateTokenAsync(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = role,
                Token = token,
                User = new CurrentUserDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.ToList(),
                    IsActive = user.IsActive
                },
                Success = true,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"]!)),
                Message = "Login successful",
                RefreshToken = null // Implement refresh token logic if needed
            };
        }

        public async Task<CurrentUserDTO> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new CurrentUserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.ToList(),
                IsActive = user.IsActive
            };
        }
    }
}
