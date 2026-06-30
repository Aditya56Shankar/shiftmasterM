using System.Collections.Generic;
using System.Threading.Tasks;
using shiftmaster.models;

namespace Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<Notification?> GetByIdAsync(int id);
        Task<Notification?> GetByIdWithEmployeeAsync(int id);
        Task AddAsync(Notification notification);
        Task SaveChangesAsync();
    }
}