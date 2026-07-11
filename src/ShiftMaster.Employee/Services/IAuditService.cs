using System;
using System.Threading.Tasks;

namespace ShiftMaster.Employee.Services
{
    public interface IAuditService
    {
        Task LogLoginAttemptAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            int statusCode,
            string authMethod = "Password",
            string? correlationId = null,
            string source = "Web",
            string? details = null,
            string? clientAppVersion = null);

        Task LogRegistrationAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            int statusCode,
            string? correlationId = null,
            string source = "Web",
            string? details = null,
            string? clientAppVersion = null);

        Task LogAuditEventAsync(
            string action,
            string entityType,
            int? recordId,
            int? userId,
            string ipAddress,
            string userAgent,
            int statusCode,
            string? details = null);
    }
}
