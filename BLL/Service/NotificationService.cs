using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.NotificationDTOs;
using DAL.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace BLL.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task AddNotificationAsync(NotificationCreateDTO notificationDto)
        {
            var notification = _mapper.Map<Notification>(notificationDto);
            await _notificationRepository.AddAsync(notification);
        }

        public async Task<List<NotificationDTO>> GetUserNotificationsAsync(string userId, bool onlyUnread = false)
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, onlyUnread);
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            await _notificationRepository.MarkAsReadAsync(notificationId);
        }
    }
} 