using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class VolunteerApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public  ApplicationUser User { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Education { get; set; }

        [Required]
        public VolunteerStatus Status { get; set; } = VolunteerStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum VolunteerStatus
    {
        Pending,
        Approved,
        Rejected,
        UnderReview
    }
}
