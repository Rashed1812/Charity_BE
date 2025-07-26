using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Models.IdentityModels
{
    public class Mediation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAvailable { get; set; } = true;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
} 