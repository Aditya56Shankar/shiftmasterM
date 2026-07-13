using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Application.DTOs;
using ShiftMaster.IdentityService.Application.Interfaces;

namespace ShiftMaster.IdentityService.Application.Services
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
                // Fallback silently if auditing service is temporarily unavailable
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
                // Fallback silently
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
                // Fallback silently
            }
        }
    }
}
