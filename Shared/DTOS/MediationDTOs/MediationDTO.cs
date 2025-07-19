using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.MediationDTOs
{
    public class MediationDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class CreateMediationDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(500)]
        public IFormFile? Image { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class UpdateMediationDTO
    {
        [StringLength(50)]
        public string FullName { get; set; }
        public IFormFile? Image { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAvailable { get; set; }
    }
} 