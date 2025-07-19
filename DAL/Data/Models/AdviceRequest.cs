using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.DTOS.AdvisorDTOs;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class AdviceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int? AdvisorId { get; set; }

        [ForeignKey("AdvisorId")]
        public virtual Advisor Advisor { get; set; }

        public int? AdvisorAvailabilityId { get; set; }

        public ConsultationType ConsultationType { get; set; } = ConsultationType.Online;

        [Required]
        public int ConsultationId { get; set; }

        [ForeignKey("ConsultationId")]
        public virtual Consultation Consultation { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public ConsultationStatus Status { get; set; } = ConsultationStatus.Pending;

        [Required]
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ConfirmedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [StringLength(2000)]
        public string Response { get; set; } = "لم يتم الرد بعد";

        public int? Rating { get; set; }

        [StringLength(500)]
        public string? Review { get; set; }
    }
}
