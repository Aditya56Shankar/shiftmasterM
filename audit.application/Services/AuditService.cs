using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ShiftMaster.CommsAuditService.Clients;
using ShiftMaster.CommsAuditService.Application.Interfaces;
using ShiftMaster.CommsAuditService.Application.DTOs;
using ShiftMaster.CommsAuditService.Infrastructure.Repositories;
using ShiftMaster.CommsAuditService.Domain.Models;

namespace ShiftMaster.CommsAuditService.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IIdentityClient _identityClient;
        private readonly IMapper _mapper;
        private readonly ILogger<AuditService> _logger;

        public AuditService(
            IAuditRepository auditRepository,
            IIdentityClient identityClient,
            IMapper mapper,
            ILogger<AuditService> logger)
        {
            _auditRepository = auditRepository;
            _identityClient = identityClient;
            _mapper = mapper;
            _logger = logger;
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

                await _auditRepository.AddAuditLogAsync(auditLog);

                _logger.LogInformation(
                    "Audit event logged: Action={Action}, EntityType={EntityType}, RecordId={RecordId}, UserId={UserId}, StatusCode={StatusCode}",
                    action,
                    entityType,
                    recordId,
                    userId,
                    statusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while logging audit event: Action={Action}, EntityType={EntityType}, StatusCode={StatusCode}",
                    action,
                    entityType,
                    statusCode);
            }
        }

        private static string GenerateCorrelationId()
        {
            return Guid.NewGuid().ToString("D").Substring(0, 8);
        }

        public async Task<IEnumerable<AuditLogDto>> GetAllAuditLogsAsync()
        {
            try
            {
                var logs = await _auditRepository.GetAllAsync();
                var dtos = _mapper.Map<List<AuditLogDto>>(logs);

                var userIds = dtos
                    .Where(l => l.UserID.HasValue)
                    .Select(l => l.UserID!.Value)
                    .Distinct()
                    .ToList();

                if (userIds.Any())
                {
                    var namesDict = await _identityClient.LookupUserNamesAsync(userIds);
                    foreach (var d in dtos)
                    {
                        if (d.UserID.HasValue && namesDict.TryGetValue(d.UserID.Value, out var name))
                        {
                            d.Actor = new ActorDto { UserID = d.UserID.Value, Name = name };
                        }
                    }
                }

                return dtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all audit logs.");
                throw;
            }
        }
    }
}
