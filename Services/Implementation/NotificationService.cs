using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using Services.DTOs;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using shiftmaster.models;

namespace Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            var notifications = await _repo.GetNotificationsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto newNotification)
        {
            var notification = _mapper.Map<Notification>(newNotification);
            await _repo.AddAsync(notification);
            await _repo.SaveChangesAsync();

            var savedNotification = await _repo.GetByIdWithEmployeeAsync(notification.NotificationID);
            return _mapper.Map<NotificationDto>(savedNotification ?? notification);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _repo.GetByIdAsync(id);
            if (notification == null) return false;

            notification.Status = NotificationStatus.Read;
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DismissNotificationAsync(int id)
        {
            var notification = await _repo.GetByIdAsync(id);
            if (notification == null) return false;

            notification.Status = NotificationStatus.Dismissed;
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}