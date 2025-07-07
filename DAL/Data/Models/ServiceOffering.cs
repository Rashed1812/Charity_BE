using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models
{
    public class ServiceOffering
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int ClickCount { get; set; } = 0;

        [StringLength(200)]
        public string ContactInfo { get; set; }

        [StringLength(500)]
        public string Requirements { get; set; }

        [StringLength(100)]
        public string Duration { get; set; }

        [StringLength(100)]
        public string Cost { get; set; }
    }
}
