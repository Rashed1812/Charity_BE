using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class AdvisorAvailability
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AdvisorId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; } = false;
        public Advisor Advisor { get; set; }
    }
}
