using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.VolunteerDTOs
{
    public class VolunteerApplicationDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Education { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }
        public string Motivation { get; set; }
        public string Availability { get; set; }
        public string Status { get; set; }
        public string AdminNotes { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string ReviewedBy { get; set; }
    }

    public class CreateVolunteerApplicationDTO
    {
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
    }

    public class UpdateVolunteerApplicationDTO
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(100)]
        public string Education { get; set; }

        [StringLength(500)]
        public string Skills { get; set; }

        [StringLength(1000)]
        public string Experience { get; set; }

        [StringLength(1000)]
        public string Motivation { get; set; }

        [StringLength(200)]
        public string Availability { get; set; }
    }

    public class ReviewVolunteerApplicationDTO
    {
        [Required]
        public string Status { get; set; }

        [StringLength(500)]
        public string AdminNotes { get; set; }
    }

    //public enum VolunteerStatusDTO
    //{
    //    Pending,
    //    Approved,
    //    Rejected,
    //    UnderReview
    //}
} 