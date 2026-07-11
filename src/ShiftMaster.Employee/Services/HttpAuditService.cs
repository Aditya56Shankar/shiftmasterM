using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftMaster.Employee.DTOs;

namespace ShiftMaster.Employee.Services
{
    public class HttpAuditService : IAuditService
    {
        private readonly HttpClient _httpClient;

        public HttpAuditService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LogLoginAttemptAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            int statusCode,
            string authMethod = "Password",
            string? correlationId = null,
            string source = "Web",
            string? details = null,
            string? clientAppVersion = null)
        {
            try
            {
                var dto = new LogLoginDto
                {
                    UserId = userId,
                    IsSuccess = isSuccess,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    StatusCode = statusCode,
                    AuthMethod = authMethod,
                    CorrelationId = correlationId,
                    Source = source,
                    Details = details,
                    ClientAppVersion = clientAppVersion
                };
                await _httpClient.PostAsJsonAsync("api/auditlogs/internal/login", dto);
            }
            catch
            {
                // Silently ignore audit logging service failures to ensure business logic continuity
            }
        }

        public async Task LogRegistrationAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            int statusCode,
            string? correlationId = null,
            string source = "Web",
            string? details = null,
            string? clientAppVersion = null)
        {
            try
            {
                var dto = new LogRegistrationDto
                {
                    UserId = userId,
                    IsSuccess = isSuccess,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    StatusCode = statusCode,
                    CorrelationId = correlationId,
                    Source = source,
                    Details = details,
                    ClientAppVersion = clientAppVersion
                };
                await _httpClient.PostAsJsonAsync("api/auditlogs/internal/registration", dto);
            }
            catch
            {
            }
        }

        public async Task LogAuditEventAsync(
            string action,
            string entityType,
            int? recordId,
            int? userId,
            string ipAddress,
            string userAgent,
            int statusCode,
            string? details = null)
        {
            try
            {
                var dto = new LogEventDto
                {
                    Action = action,
                    EntityType = entityType,
                    RecordId = recordId,
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    StatusCode = statusCode,
                    Details = details
                };
                await _httpClient.PostAsJsonAsync("api/auditlogs/internal/event", dto);
            }
            catch
            {
            }
        }
    }
}
