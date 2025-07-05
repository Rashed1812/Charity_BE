using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.AdvisorDTOs
{
    public class AdvisorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialty { get; set; }
        public string? Description { get; set; }
        public string? ZoomRoomUrl { get; set; }
        public string ConsultationName { get; set; }
        public List<AdvisorAvailabilityDTO> Availabilities { get; set; }
        //public List<AdvisorRequestDTO> AdviceRequests { get; set; }
    }
}
