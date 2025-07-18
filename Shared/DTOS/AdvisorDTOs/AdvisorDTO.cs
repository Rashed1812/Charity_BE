﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.AdvisorDTOs
{
    public class AdvisorDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
<<<<<<< Updated upstream
        public string fullName { get; set; }
=======
        public string FirstName { get; set; }
        public string LastName { get; set; }
>>>>>>> Stashed changes
        public string Specialty { get; set; }
        public string Description { get; set; }
        public string ZoomRoomUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string email { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ConsultationId { get; set; }
        public string ConsultationName { get; set; }
        
        // Statistics
        public int TotalConsultations { get; set; }
        public int PendingRequests { get; set; }
        public double AverageRating { get; set; }
    }

    public class CreateAdvisorDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string ZoomRoomUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int? ConsultationId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
<<<<<<< Updated upstream

        //[StringLength(500)]
        //public string? ImageUrl { get; set; }
        
        public IFormFile? Image { get; set; }

        public ConsultationType ConsultationType { get; set; } = ConsultationType.Online;
=======
>>>>>>> Stashed changes
    }

    public class UpdateAdvisorDTO
    {
        [StringLength(50)]
        public string FullName { get; set; }


        [StringLength(100)]
        public string Specialty { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string ZoomRoomUrl { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int? ConsultationId { get; set; }

        public bool? IsActive { get; set; }
<<<<<<< Updated upstream

        //[StringLength(500)]
        //public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }

        public ConsultationType? ConsultationType { get; set; }
    }

    public class AdvisorAvailabilityDTO
    {
        public int Id { get; set; }
        public int AdvisorId { get; set; }
        public string AdvisorName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan Duration { get; set; }
        public ConsultationType ConsultationType { get; set; }
        public bool IsBooked { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
=======
>>>>>>> Stashed changes
    }

    public class CreateAvailabilityDTO
    {
        [Required]
        public int AdvisorId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }
    }

    public class UpdateAvailabilityDTO
    {
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool? IsAvailable { get; set; }
        public string Notes { get; set; }
    }

    public class BulkAvailabilityDTO
    {
        [Required]
        public int AdvisorId { get; set; }

        [Required]
        public List<CreateAvailabilityDTO> Availabilities { get; set; }
    }

    public enum ConsultationStatus
    {
        Pending = 0,
        Confirmed = 1,
        Completed = 2,
        Cancelled = 3,
        InProgress = 4
    }
}
