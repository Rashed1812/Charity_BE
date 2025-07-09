namespace Shared.DTOS.NotificationDTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Type { get; set; }
    }

    public class NotificationCreateDTO
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; } = NotificationType.General;
    }
    public enum NotificationType
    {
        General = 0,
        Complaint = 1,
        Consultation = 2,
        Appointment = 3
    }
} 