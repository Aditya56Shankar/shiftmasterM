using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces
{
    public interface IAuditRepository
    {
        Task AddAuditLogAsync(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> GetAllAsync();
    }
}
