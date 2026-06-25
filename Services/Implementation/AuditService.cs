using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    /// <summary>
    /// Service implementation for audit logging of authentication events and other critical operations.
    /// </summary>
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger<AuditService> _logger;

        public AuditService(IAuditRepository auditRepository, ILogger<AuditService> logger)
        {
            _auditRepository = auditRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task LogLoginAttemptAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            string authMethod = "Password",
            string correlationId = null,
            string source = "Web",
            string details = null,
            string clientAppVersion = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Action = isSuccess ? "UserLogin" : "UserLoginFailed",
                    EntityType = "User",
                    RecordID = userId,
                    UserID = userId,
                    Timestamp = DateTime.UtcNow,
                    IsSuccess = isSuccess,
                    IPAddress = ipAddress,
                    UserAgent = userAgent,
                    AuthMethod = authMethod,
                    CorrelationId = correlationId ?? GenerateCorrelationId(),
                    Source = source,
                    Details = details,
                    ClientAppVersion = clientAppVersion
                };

                // Delegated to the repository
                await _auditRepository.AddAuditLogAsync(auditLog);

                var logLevel = isSuccess ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(
                    logLevel,
                    "Login attempt logged: UserId={UserId}, Success={Success}, CorrelationId={CorrelationId}",
                    userId,
                    isSuccess,
                    auditLog.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while logging login attempt: UserId={UserId}, Success={Success}",
                    userId,
                    isSuccess);
                // Do not throw - audit log failures should not block authentication
            }
        }

        /// <inheritdoc/>
        public async Task LogRegistrationAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            string correlationId = null,
            string source = "Web",
            string details = null,
            string clientAppVersion = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Action = isSuccess ? "UserRegister" : "UserRegisterFailed",
                    EntityType = "User",
                    RecordID = userId,
                    UserID = userId,
                    Timestamp = DateTime.UtcNow,
                    IsSuccess = isSuccess,
                    IPAddress = ipAddress,
                    UserAgent = userAgent,
                    AuthMethod = "SelfRegister",
                    CorrelationId = correlationId ?? GenerateCorrelationId(),
                    Source = source,
                    Details = details,
                    ClientAppVersion = clientAppVersion
                };

                // Delegated to the repository
                await _auditRepository.AddAuditLogAsync(auditLog);

                var logLevel = isSuccess ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(
                    logLevel,
                    "Registration logged: UserId={UserId}, Success={Success}, CorrelationId={CorrelationId}",
                    userId,
                    isSuccess,
                    auditLog.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while logging registration: UserId={UserId}, Success={Success}",
                    userId,
                    isSuccess);
                // Do not throw - audit log failures should not block registration
            }
        }

        /// <inheritdoc/>
        public async Task LogAuditEventAsync(
            string action,
            string entityType,
            int? recordId,
            int? userId,
            string ipAddress,
            string userAgent,
            string details = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Action = action,
                    EntityType = entityType,
                    RecordID = recordId,
                    UserID = userId,
                    Timestamp = DateTime.UtcNow,
                    IsSuccess = true,
                    IPAddress = ipAddress,
                    UserAgent = userAgent,
                    CorrelationId = GenerateCorrelationId(),
                    Details = details
                };

                // Delegated to the repository
                await _auditRepository.AddAuditLogAsync(auditLog);

                _logger.LogInformation(
                    "Audit event logged: Action={Action}, EntityType={EntityType}, RecordId={RecordId}, UserId={UserId}",
                    action,
                    entityType,
                    recordId,
                    userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while logging audit event: Action={Action}, EntityType={EntityType}",
                    action,
                    entityType);
                // Do not throw - audit log failures should not block the primary operation
            }
        }

        /// <summary>
        /// Generate a unique correlation ID for tracing requests across the system.
        /// </summary>
        private static string GenerateCorrelationId()
        {
            return Guid.NewGuid().ToString("D").Substring(0, 8); // 8-char GUID part
        }
        public async Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync()
        {
            try
            {
                return await _auditRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all audit logs.");
                throw; // Rethrow because the controller needs to know it failed
            }
        }
    }
}