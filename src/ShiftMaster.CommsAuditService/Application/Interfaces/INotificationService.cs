using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.CommsAuditService.Application.DTOs;

namespace ShiftMaster.CommsAuditService.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto newNotification);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DismissNotificationAsync(int id);
    }
}
