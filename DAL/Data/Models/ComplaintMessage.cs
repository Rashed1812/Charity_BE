using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models
{
    public class ComplaintMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ComplaintId { get; set; }

        [ForeignKey("ComplaintId")]
        public Complaint Complaint { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
