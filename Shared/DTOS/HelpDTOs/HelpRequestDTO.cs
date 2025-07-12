using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.HelpDTOs
{
    public class HelpRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public int HelpTypeId { get; set; }
        public string HelpTypeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateHelpRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(1000)]
        public string Notes { get; set; }

        [Required]
        public int HelpTypeId { get; set; }
    }
} 