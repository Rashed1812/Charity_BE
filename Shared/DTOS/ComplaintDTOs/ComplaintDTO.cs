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
        public string Category { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string Resolution { get; set; }
        public int MessageCount { get; set; }
    }

    public class ComplaintWithMessagesDTO : ComplaintDTO
    {
        public List<ComplaintMessageDTO> Messages { get; set; }
    }

    public class CreateComplaintDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Priority { get; set; }
    }

    public class UpdateComplaintDTO
    {
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public string Category { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Resolution { get; set; }
    }

    public class ComplaintMessageDTO
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateComplaintMessageDTO
    {
        [Required]
        [StringLength(2000)]
        public string Message { get; set; }
    }

    public enum ComplaintStatus
    {
        Pending,
        InProgress,
        Resolved,
        Closed
    }
} 