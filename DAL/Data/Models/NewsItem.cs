using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class NewsItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
