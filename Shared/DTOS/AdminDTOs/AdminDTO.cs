using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.AdminDTOs
{
    public class AdminDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateAdminDTO
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
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class UpdateAdminDTO
    {
        [StringLength(50)]
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }

    public class SuspendUserDTO
    {
        [Required]
        public string Reason { get; set; }
        public DateTime? Until { get; set; }
    }

    public class SendNotificationDTO
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public List<string> UserIds { get; set; } = new List<string>();
    }
} 