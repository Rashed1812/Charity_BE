using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.LectureDTOs
{
    public class LectureDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Speaker { get; set; }
        public string Type { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int Duration { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public int? ConsultationId { get; set; }
        public string ConsultationName { get; set; }
        public List<string> Tags { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }

    public class CreateLectureDTO
    {
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
        public string ThumbnailUrl { get; set; }

        public int? ConsultationId { get; set; }

        public List<string> Tags { get; set; }
    }

    public class UpdateLectureDTO
    {
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Speaker { get; set; }

        public LectureType? Type { get; set; }

        [StringLength(500)]
        public string VideoUrl { get; set; }

        [StringLength(500)]
        public string ThumbnailUrl { get; set; }

        public int? ConsultationId { get; set; }

        public List<string> Tags { get; set; }
    }

    public class LectureUploadDTO
    {
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

        public int? ConsultationId { get; set; }

        public List<string> Tags { get; set; }
    }

    public class LectureSearchDTO
    {
        public string SearchTerm { get; set; }
        public LectureType? Type { get; set; }
        public int? ConsultationId { get; set; }
        public string Speaker { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public enum LectureType
    {
        Video,
        Audio,
        Document,
        Presentation
    }
} 