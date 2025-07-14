using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.AuthDTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Role { get; set; } = "User"; // Default role is User
        public bool IsActive { get; set; } = true; // Default is active
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null; // Updated at can be null initially
        public string UserName { get; set; }
    }

    public class RegisterAdminDTO
    {

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string? Department { get; set; }

        [Required]
        [StringLength(100)]
        public string? Position { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterAdvisorDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int? ConsultationId { get; set; }
        public string? ZoomRoomUrl { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class AuthResponseDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public CurrentUserDTO User { get; set; }
    }

    public class CurrentUserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Role { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
} 