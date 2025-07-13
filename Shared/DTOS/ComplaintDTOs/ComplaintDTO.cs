using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.ComplaintDTOs
{

    public class ComplaintDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ComplaintCategory Category { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Resolution { get; set; }
    }

    public class CreateComplaintDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public ComplaintCategory Category { get; set; }

        [Required]
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";
    }

    public class UpdateComplaintDTO
    {
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
        public ComplaintCategory? Category { get; set; }
        [StringLength(20)]
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public string? Resolution { get; set; }
    }
    public enum ComplaintStatus
    {
        Pending,
        InProgress,
        Resolved,
        Closed
    }
    public enum ComplaintCategory
    {
        Employee = 0, // شكوى عن موظف
        Service = 1,   // شكوى عن خدمة
        Facility = 2,  // شكوى عن مرفق
        Other = 3      // أخرى
    }
}