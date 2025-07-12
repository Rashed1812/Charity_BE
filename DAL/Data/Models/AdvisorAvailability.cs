using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.AdvisorDTOs;

namespace DAL.Data.Models
{
    public class AdvisorAvailability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdvisorId { get; set; }

        [Required]
        public DateTime Date { get; set; } // اليوم المتاح

        [Required]
        public TimeSpan Time { get; set; } // الساعة المتاحة

        [Required]
        public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(30);

        public ConsultationType ConsultationType { get; set; } = ConsultationType.Online;

        public bool IsBooked { get; set; } = false;

        public int? AdviceRequestId { get; set; } // ربط اختياري بالحجز

        [StringLength(200)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey("AdvisorId")]
        public virtual Advisor Advisor { get; set; }
        [ForeignKey("AdviceRequestId")]
        public virtual AdviceRequest? AdviceRequest { get; set; }
    }
}
