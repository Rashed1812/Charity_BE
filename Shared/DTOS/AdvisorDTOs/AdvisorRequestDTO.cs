using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.AdvisorDTOs
{
    public class AdvisorRequestDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public int AdvisorId { get; set; }
        public string AdvisorFullName { get; set; }
        public int ConsultationId { get; set; }
        public string ConsultationName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string? Notes { get; set; }          
        public string Status { get; set; }
    }

}
