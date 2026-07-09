using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    /// Service implementation for audit logging of authentication events and other critical operations.
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger<AuditService> _logger;

        public AuditService(IAuditRepository auditRepository, ILogger<AuditService> logger)
        {
            _auditRepository = auditRepository;
            _logger = logger;
        }

        public async Task LogLoginAttemptAsync(int? userId, bool isSuccess, string ipAddress, string userAgent, int statusCode, string authMethod = "Password", string correlationId = null, string source = "Web", string details = null, string clientAppVersion = null)
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
                    StatusCode = statusCode,
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

        public async Task LogRegistrationAsync(int? userId, bool isSuccess, string ipAddress, string userAgent, int statusCode, string correlationId = null, string source = "Web", string details = null,string clientAppVersion = null)
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
                    StatusCode = statusCode,
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

        public async Task LogAuditEventAsync(string action, string entityType, int? recordId, int? userId, string ipAddress, string userAgent, int statusCode, string details = null)
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
                    IsSuccess = statusCode >= 200 && statusCode <= 299,
                    IPAddress = ipAddress,
                    UserAgent = userAgent,
                    CorrelationId = GenerateCorrelationId(),
                    Details = details,
                    StatusCode = statusCode
                };

                // Delegated to the repository
                await _auditRepository.AddAuditLogAsync(auditLog);

                _logger.LogInformation(
                    "Audit event logged: Action={Action}, EntityType={EntityType}, RecordId={RecordId}, UserId={UserId}, StatusCode={StatusCode}",
                    action,
                    entityType,
                    recordId,
                    userId, statusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while logging audit event: Action={Action}, EntityType={EntityType}, StatusCode={StatusCode}",
                    action,
                    entityType, statusCode);
                // Do not throw - audit log failures should not block the primary operation
            }
        }

        /// Generate a unique correlation ID for tracing requests across the system.
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