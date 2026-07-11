using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.CommsAuditService.Domain.Models;
using ShiftMaster.CommsAuditService.Infrastructure.Data;

namespace ShiftMaster.CommsAuditService.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CommsAuditDbContext _context;

        public NotificationRepository(CommsAuditDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
