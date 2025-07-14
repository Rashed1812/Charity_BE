using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.ComplaintDTOs;

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
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;

        [Required]
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        [StringLength(2000)]
        public string? Resolution { get; set; }

        // Navigation Properties
    }

    //public enum ComplaintStatus
    //{
    //    Pending,
    //    InProgress,
    //    Resolved,
    //    Closed
    //}
}

