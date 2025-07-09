using System;
using System.ComponentModel.DataAnnotations;
using Shared.DTOS.NotificationDTOs;

namespace DAL.Data.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // صاحب الإشعار (مستخدم، أدمن، مستشار...)

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public NotificationType Type { get; set; } = NotificationType.General;
    }


} 