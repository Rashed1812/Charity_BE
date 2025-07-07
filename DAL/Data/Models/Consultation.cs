using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class Consultation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ConsultationName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Advisor> Advisors { get; set; } = new List<Advisor>();
        public virtual ICollection<AdviceRequest> AdviceRequests { get; set; } = new List<AdviceRequest>();
        public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
    }
}
