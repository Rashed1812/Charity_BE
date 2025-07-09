using Shared.DTOS.NotificationDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface INotificationService
    {
        Task AddNotificationAsync(NotificationCreateDTO notificationDto);
        Task<List<NotificationDTO>> GetUserNotificationsAsync(string userId, bool onlyUnread = false);
        Task MarkAsReadAsync(int notificationId);
    }
} 