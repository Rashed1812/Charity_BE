using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public string Title { get; set; }

        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ComplaintMessage> Messages { get; set; }
    }
    public enum ComplaintStatus
    {
        Pending = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
    }
}

