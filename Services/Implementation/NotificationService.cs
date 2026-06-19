using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.Employee)
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new NotificationDto
                {
                    NotificationID = n.NotificationID,
                    UserID = n.UserID,
                    EmployeeName = n.Employee != null ? n.Employee.Name : "Unknown Employee",
                    Message = n.Message,
                    Category = n.Category.ToString(),
                    Status = n.Status.ToString(),
                    CreatedDate = n.CreatedDate
                }).ToListAsync();
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto newNotification)
        {
            var notification = new Notification
            {
                UserID = newNotification.UserID,
                Message = newNotification.Message,
                Category = Enum.Parse<NotificationCategory>(newNotification.Category, true),
                Status = NotificationStatus.Unread, // Defaults to Unread on creation
                CreatedDate = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Refresh to grab the hydrated Employee Name for the returned DTO
            var savedNotification = await _context.Notifications
                .Include(n => n.Employee)
                .FirstOrDefaultAsync(n => n.NotificationID == notification.NotificationID);

            return new NotificationDto
            {
                NotificationID = notification.NotificationID,
                UserID = notification.UserID,
                EmployeeName = savedNotification?.Employee?.Name ?? "Unknown Employee",
                Message = notification.Message,
                Category = notification.Category.ToString(),
                Status = notification.Status.ToString(),
                CreatedDate = notification.CreatedDate
            };
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            notification.Status = NotificationStatus.Read;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DismissNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            notification.Status = NotificationStatus.Dismissed;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}