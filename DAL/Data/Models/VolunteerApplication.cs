using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class VolunteerApplication
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? AreaOfInterest { get; set; }

        public string? Notes { get; set; }

        public VolunteerStatus Status { get; set; } = VolunteerStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    public enum VolunteerStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
