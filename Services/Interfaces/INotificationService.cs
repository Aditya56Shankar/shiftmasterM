using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;

namespace Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto newNotification);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DismissNotificationAsync(int id);
    }
}