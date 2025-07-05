using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class ComplaintMessage
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [Required]
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsFromAdmin { get; set; }
    }
}
