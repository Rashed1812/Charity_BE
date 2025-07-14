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
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [StringLength(100)]
        public string Education { get; set; }

        [Required]
        [StringLength(500)]
        public string Skills { get; set; }

        [Required]
        [StringLength(1000)]
        public string Experience { get; set; }

        [Required]
        [StringLength(1000)]
        public string Motivation { get; set; }

        [Required]
        [StringLength(200)]
        public string Availability { get; set; }

        [Required]
        public VolunteerStatus Status { get; set; } = VolunteerStatus.Pending;

        [StringLength(500)]
        public string AdminNotes { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        [StringLength(450)]
        public string ReviewedBy { get; set; }
    }

    public enum VolunteerStatus
    {
        Pending,
        Approved,
        Rejected,
        UnderReview
    }
}
