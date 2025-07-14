using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models.IdentityModels
{
    public class Advisor
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

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? ZoomRoomUrl { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsAvailable { get; set; } = true;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int? ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

        // Navigation Properties
        public ICollection<AdvisorAvailability> Availabilities { get; set; } = new List<AdvisorAvailability>();
        public ICollection<AdviceRequest> AdviceRequests { get; set; } = new List<AdviceRequest>();
    }
}
