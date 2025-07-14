using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.ServiceOfferingDTOs
{
    public class ServiceOfferingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ClickCount { get; set; }
        public string ContactInfo { get; set; }
        public string Requirements { get; set; }
    }

    public class CreateServiceOfferingDTO
    {
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

        public bool IsActive { get; set; }

        [StringLength(200)]
        public string ContactInfo { get; set; }

        [StringLength(500)]
        public string Requirements { get; set; }

    }

    public class UpdateServiceOfferingDTO
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(200)]
        public string ContactInfo { get; set; }

        [StringLength(500)]
        public string Requirements { get; set; }
    }
} 