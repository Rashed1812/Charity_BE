using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? ZoomRoomUrl { get; set; }

        // Navigation
        public int ConsultationId { get; set; }
        public Consultation Consultation { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<AdvisorAvailability> Availabilities { get; set; }
        public ICollection<AdviceRequest> AdviceRequests { get; set; }
    }
}
