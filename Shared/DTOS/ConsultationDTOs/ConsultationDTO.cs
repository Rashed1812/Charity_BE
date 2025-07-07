using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.AdvisorDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.ConsultationDTOs
{
    public class ConsultationDTO
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Consultation name cannot exceed 100 characters")]
        public string ConsultationName { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Related data
        public int AdvisorCount { get; set; }
        public int RequestCount { get; set; }
        public int LectureCount { get; set; }
    }

    public class UpdateConsultationDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Consultation name cannot exceed 100 characters")]
        public string ConsultationName { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
        
        public bool? IsActive { get; set; }
    }

    public class ConsultationWithAdvisorsDTO : ConsultationDTO
    {
        public List<AdvisorSummaryDTO> Advisors { get; set; } = new();
    }

    public class AdvisorSummaryDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int ActiveRequestCount { get; set; }
    }
}
