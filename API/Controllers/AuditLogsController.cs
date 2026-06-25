using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace ShiftMaster.Controllers
{
    [Authorize(Roles = "Admin,Supervisor")]
    [ApiController]
    [Route("api/auditlogs")]
    [Produces("application/json")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditLogsController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuditLogs()
        {
            // Delegated to the service layer
            var logs = await _auditService.GetAllAuditLogsAsync();

            return Ok(logs);
        }
    }
}