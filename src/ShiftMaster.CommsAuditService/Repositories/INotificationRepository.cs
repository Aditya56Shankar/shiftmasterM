using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.CommsAuditService.Models;

namespace ShiftMaster.CommsAuditService.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<Notification?> GetByIdAsync(int id);
        Task AddAsync(Notification notification);
        Task SaveChangesAsync();
    }
}
