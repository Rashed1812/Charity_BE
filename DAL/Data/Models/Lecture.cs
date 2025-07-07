using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.LectureDTOs;

namespace DAL.Data.Models
{
    public class Lecture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Speaker { get; set; }

        [Required]
        public LectureType Type { get; set; }

        [StringLength(500)]
        public string VideoUrl { get; set; }

        [StringLength(500)]
        public string? ThumbnailUrl { get; set; }

        [StringLength(500)]
        public string FilePath { get; set; }

        public long? FileSize { get; set; }

        [StringLength(50)]
        public string FileFormat { get; set; }

        public int? Duration { get; set; } // in seconds

        public bool IsPublished { get; set; } = false;

        public LectureStatus Status { get; set; } = LectureStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PublishedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int ViewCount { get; set; } = 0;

        public int DownloadCount { get; set; } = 0;

        public int? ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

        [StringLength(1000)]
        public string Tags { get; set; } // JSON array of tags

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public ApplicationUser CreatedByUser { get; set; }
    }

    //public enum LectureType
    //{
    //    Video,
    //    Audio,
    //    Document,
    //    Presentation
    //}

    public enum LectureStatus
    {
        Draft,
        Published,
        Archived,
        Pending
    }
} 