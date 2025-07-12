using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.HelpDTOs
{
    public class HelpTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateHelpTypeDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
} 