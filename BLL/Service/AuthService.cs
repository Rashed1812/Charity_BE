using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using BLL.DTOS.AuthDTO;
using BLL.ServiceAbstraction;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

public class AuthService(
    UserManager<ApplicationUser> _userManager,
    IConfiguration _config,
    IAdminRepository _adminRepository,
    IAdvisorRepository _advisorRepository
) : IAuthService
{
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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
        var siginCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(double.Parse(_config["JWT:DurationInDays"]!)),
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
            PhoneNumber = dto.PhoneNumber
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
            Address = dto.Address
        };

        await _adminRepository.AddAsync(admin);
        await _adminRepository.SaveChangesAsync();

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
            PhoneNumber = dto.PhoneNumber
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
            ZoomRoomUrl = dto.ZoomRoomUrl
        };

        await _advisorRepository.AddAsync(advisor);
        await _advisorRepository.SaveChangesAsync();

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
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, "Patient");

        var token = await CreateTokenAsync(user);

        return new AuthResponseDTO
        {
            FullName = user.FullName,
            Email = user.Email,
            Role = "Patient",
            Token = token
        };
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
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
            Token = token
        };
    }

    public async Task<CurrentUserDTO> GetCurrentUserAsync(string email)
    {

        var user = await _userManager.Users
        .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null!;

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return new CurrentUserDTO
        {
            Email = user.Email!,
            FullName = user.FullName,
            DisplayName = user.FullName,
            Role = role,
            Token = await CreateTokenAsync(user)
        };
    }
}
