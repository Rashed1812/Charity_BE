using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.AdvisorDTOs
{
    public class AdvisorAvailabilityDTO
    {
        public int Id { get; set; }
        public int AdvisorId { get; set; }
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsBooked { get; set; }
    }
}
