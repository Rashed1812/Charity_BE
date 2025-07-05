using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class AdviceRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int AdvisorId { get; set; }
        public Advisor Advisor { get; set; }

        public int ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

        public DateTime AppointmentTime { get; set; }   
        public string? Notes { get; set; }

        public ConsultationStatus Status { get; set; } = ConsultationStatus.Pending;
    }
    public enum ConsultationStatus
    {
        Pending = 0,
        Confirmed = 1,
        Completed = 2,
        Cancelled = 3
    }
}
