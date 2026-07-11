using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.CommsAuditService.Models;

namespace ShiftMaster.CommsAuditService.Repositories
{
    public interface IAuditRepository
    {
        Task AddAuditLogAsync(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> GetAllAsync();
    }
}
