using shiftmaster.models;

namespace Services.Interfaces
{/// <summary>
 /// Interface for audit logging of authentication events and other critical operations.
 /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Logs an attempt to log into the system.
        /// </summary>
        Task LogLoginAttemptAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            int statusCode,
            string authMethod = "Password",
            string correlationId = null,
            string source = "Web",
            string details = null,
            string clientAppVersion = null);

        /// <summary>
        /// Logs a user registration event.
        /// </summary>
        Task LogRegistrationAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            int statusCode,
            string correlationId = null,
            string source = "Web",
            string details = null,
            string clientAppVersion = null);

        /// <summary>
        /// Logs a generic audit event for general system operations.
        /// </summary>
        Task LogAuditEventAsync(
            string action,
            string entityType,
            int? recordId,
            int? userId,
            string ipAddress,
            string userAgent,
            int statusCode,
            string details = null);
        // discussion, remember to add it here as well:
        // Task<IEnumerable<AuditLogDto>> GetAllAuditLogsAsync();
        Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync();
    }
}
