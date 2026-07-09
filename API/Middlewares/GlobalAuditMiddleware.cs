using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using System.Diagnostics;
namespace shiftMaster.API.Middlewares
{
    public class GlobalAuditMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalAuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuditService auditService)
        {
            if (context.Request.Path.StartsWithSegments("/api/users"))
            {
                await _next(context);
                return;
            }
            var stopwatch = Stopwatch.StartNew();

            // Let the request process normally
            await _next(context);

            stopwatch.Stop();

            // After request finishes, log it
            var statusCode = context.Response.StatusCode;
            var method = context.Request.Method;
            var path = context.Request.Path;
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Convert string userId to int? if applicable
            int? parsedUserId = int.TryParse(userId, out var id) ? id : null;

            var details = $"Path: {path} | Method: {method} | ExecutionTime: {stopwatch.ElapsedMilliseconds}ms";

            // Modify your IAuditService.LogAuditEventAsync to accept a statusCode parameter
            await auditService.LogAuditEventAsync(
                action: $"HTTP {method}",
                entityType: "System",
                recordId: null,
                userId: parsedUserId,
                ipAddress: ip,
                userAgent: userAgent,
                details: details,
                statusCode: statusCode // Make sure to add this parameter to your service/repo
            );
        }
    }
}
