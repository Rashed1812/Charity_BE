using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class Consultation
    {
        public int Id { get; set; }
        public string ConsultationName { get; set; }
        public ICollection<Advisor> Advisors { get; set; }
        public ICollection<AdviceRequest> AdviceRequests { get; set; }
    }
}
