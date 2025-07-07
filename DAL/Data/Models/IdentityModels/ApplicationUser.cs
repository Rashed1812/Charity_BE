using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL.Data.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(500)]
        public string? ProfilePictureUrl { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Advisor? Advisor { get; set; }
        public Admin? Admin { get; set; }
        public virtual ICollection<AdviceRequest> AdviceRequests { get; set; } = new List<AdviceRequest>();
        public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
        public virtual ICollection<ComplaintMessage> ComplaintMessages { get; set; } = new List<ComplaintMessage>();
        public virtual ICollection<VolunteerApplication> VolunteerApplications { get; set; } = new List<VolunteerApplication>();
        public virtual ICollection<Lecture> CreatedLectures { get; set; } = new List<Lecture>();
    }
}
