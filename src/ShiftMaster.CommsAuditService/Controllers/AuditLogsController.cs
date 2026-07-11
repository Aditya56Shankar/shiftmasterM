using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.CommsAuditService.DTOs;
using ShiftMaster.CommsAuditService.Services;

namespace ShiftMaster.CommsAuditService.Controllers
{
    [ApiController]
    [Route("api/auditlogs")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditLogsController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> GetAllAuditLogs()
        {
            var logs = await _auditService.GetAllAuditLogsAsync();
            return Ok(logs);
        }

        // ===================================================
        // INTERNAL UN-AUTHENTICATED ENDPOINTS FOR OTHER SERVICES
        // ===================================================

        [HttpPost("internal/login")]
        public async Task<IActionResult> LogLoginAttempt([FromBody] LogLoginAttemptDto dto)
        {
            await _auditService.LogLoginAttemptAsync(
                dto.UserId,
                dto.IsSuccess,
                dto.IpAddress,
                dto.UserAgent,
                dto.StatusCode,
                dto.AuthMethod,
                dto.CorrelationId,
                dto.Source,
                dto.Details,
                dto.ClientAppVersion);

            return Ok();
        }

        [HttpPost("internal/registration")]
        public async Task<IActionResult> LogRegistration([FromBody] LogRegistrationInternalDto dto)
        {
            await _auditService.LogRegistrationAsync(
                dto.UserId,
                dto.IsSuccess,
                dto.IpAddress,
                dto.UserAgent,
                dto.StatusCode,
                dto.CorrelationId,
                dto.Source,
                dto.Details,
                dto.ClientAppVersion);

            return Ok();
        }

        [HttpPost("internal/event")]
        public async Task<IActionResult> LogAuditEvent([FromBody] LogEventInternalDto dto)
        {
            await _auditService.LogAuditEventAsync(
                dto.Action,
                dto.EntityType,
                dto.RecordId,
                dto.UserId,
                dto.IpAddress,
                dto.UserAgent,
                dto.StatusCode,
                dto.Details);

            return Ok();
        }
    }

    // Internal request payload structures
    public class LogLoginAttemptDto
    {
        public int? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string AuthMethod { get; set; } = "Password";
        public string? CorrelationId { get; set; }
        public string Source { get; set; } = "Web";
        public string? Details { get; set; }
        public string? ClientAppVersion { get; set; }
    }

    public class LogRegistrationInternalDto
    {
        public int? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string? CorrelationId { get; set; }
        public string Source { get; set; } = "Web";
        public string? Details { get; set; }
        public string? ClientAppVersion { get; set; }
    }

    public class LogEventInternalDto
    {
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public int? RecordId { get; set; }
        public int? UserId { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string? Details { get; set; }
    }
}
