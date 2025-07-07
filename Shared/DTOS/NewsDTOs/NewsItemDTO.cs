using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.NewsDTOs
{
    public class NewsItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public List<string> Tags { get; set; }
    }

    public class CreateNewsItemDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; }

        [Required]
        [StringLength(500)]
        public string Summary { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        public bool IsPublished { get; set; }
        public List<string> Tags { get; set; }
    }

    public class UpdateNewsItemDTO
    {
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(5000)]
        public string Content { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public bool? IsPublished { get; set; }
        public List<string> Tags { get; set; }
    }
} 