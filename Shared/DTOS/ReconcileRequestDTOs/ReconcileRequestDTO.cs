using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.ReconcileRequestDTOs
{
    public class ReconcileRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RequestText { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateReconcileRequestDTO
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
        [StringLength(2000)]
        public string RequestText { get; set; }
    }
} 