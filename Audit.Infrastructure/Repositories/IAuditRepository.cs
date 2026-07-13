using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.CommsAuditService.Domain.Models;

namespace ShiftMaster.CommsAuditService.Infrastructure.Repositories
{
    public interface IAuditRepository
    {
        Task AddAuditLogAsync(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> GetAllAsync();
    }
}
