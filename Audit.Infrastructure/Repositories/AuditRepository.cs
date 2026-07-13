using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.CommsAuditService.Domain.Models;
using ShiftMaster.CommsAuditService.Infrastructure.Data;

namespace ShiftMaster.CommsAuditService.Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IDbContextFactory<CommsAuditDbContext> _contextFactory;

        public AuditRepository(IDbContextFactory<CommsAuditDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAuditLogAsync(AuditLog auditLog)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.AuditLogs.Add(auditLog);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            using (var context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.AuditLogs
                    .AsNoTracking()
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();
            }
        }
    }
}
