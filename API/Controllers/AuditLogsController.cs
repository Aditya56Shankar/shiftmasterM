using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Services.DTOs; // <- use your Services DTO namespace
using System.Linq;

namespace ShiftMaster.Controllers
{
    [Authorize(Roles = "Shift Supervisior")]
    [ApiController]
    [Route("api/auditlogs")]
    [Produces("application/json")]
    public class AuditLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuditLogs()
        {
            var logs = await _context.AuditLogs
                .Select(a => new AuditLogDto
                {
                    AuditID = a.AuditID,
                    Action = a.Action,
                    EntityType = a.EntityType,
                    RecordID = a.RecordID,
                    Timestamp = a.Timestamp,
                    UserID = a.UserID,
                    Actor = a.Actor == null ? null : new ActorDto
                    {
                        UserID = a.Actor.UserID,
                        Name = a.Actor.Name
                    },
                    IsSuccess = a.IsSuccess,
                    IPAddress = a.IPAddress,
                    UserAgent = a.UserAgent,
                    Details = a.Details,
                    AuthMethod = a.AuthMethod,
                    CorrelationId = a.CorrelationId,
                    Source = a.Source,
                    ClientAppVersion = a.ClientAppVersion
                })
                .ToListAsync();

            return Ok(logs);
        }
    }
}