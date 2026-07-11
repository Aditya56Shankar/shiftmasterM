using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ShiftMaster.CommsAuditService.Clients;
using ShiftMaster.CommsAuditService.Application.Interfaces;
using ShiftMaster.CommsAuditService.Application.DTOs;
using ShiftMaster.CommsAuditService.Domain.Enums;
using ShiftMaster.CommsAuditService.Domain.Models;
using ShiftMaster.CommsAuditService.Infrastructure.Repositories;

namespace ShiftMaster.CommsAuditService.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IIdentityClient _identityClient;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repo, IIdentityClient identityClient, IMapper mapper)
        {
            _repo = repo;
            _identityClient = identityClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            var notifications = await _repo.GetNotificationsByUserIdAsync(userId);
            var dtos = _mapper.Map<List<NotificationDto>>(notifications);

            var userName = await _identityClient.GetUserNameAsync(userId);
            foreach (var d in dtos)
            {
                d.EmployeeName = userName;
            }

            return dtos;
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto newNotification)
        {
            var notification = _mapper.Map<Notification>(newNotification);
            await _repo.AddAsync(notification);
            await _repo.SaveChangesAsync();

            var userName = await _identityClient.GetUserNameAsync(notification.UserID);
            var dto = _mapper.Map<NotificationDto>(notification);
            dto.EmployeeName = userName;

            return dto;
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
