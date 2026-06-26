using System.Threading.Tasks;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using shiftmaster.models;

namespace Data.Implementation
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public AuditRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAuditLogAsync(AuditLog auditLog)
        {
            // The DbContext scope is managed here
            using (var context = _contextFactory.CreateDbContext())
            {
                context.AuditLogs.Add(auditLog);
                await context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            // Create a short-lived context and ensure it gets disposed
            using var context = await _contextFactory.CreateDbContextAsync();

            return await context.AuditLogs
                .AsNoTracking()
                .OrderByDescending(a => a.Timestamp) // Order by newest first
                .ToListAsync();
        }
    }
}