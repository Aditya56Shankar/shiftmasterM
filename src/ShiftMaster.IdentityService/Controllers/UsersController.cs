using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.IdentityService.DTOs;
using ShiftMaster.IdentityService.Services;
using ShiftMaster.IdentityService.Data;
using Microsoft.EntityFrameworkCore;

namespace ShiftMaster.IdentityService.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ProducesResponseType(200, Type = typeof(object))]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuditService _auditService;
        private readonly IdentityDbContext _context;

        public UsersController(IAuthService authService, IAuditService auditService, IdentityDbContext context)
        {
            _authService = authService;
            _auditService = auditService;
            _context = context;
        }

        private string GetClientIpAddress() =>
            HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor)
                ? xForwardedFor.ToString().Split(',')[0].Trim()
                : HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        private string GetUserAgent() =>
            HttpContext.Request.Headers["User-Agent"].ToString() ?? "Unknown";

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ipAddress = GetClientIpAddress();
            var userAgent = GetUserAgent();

            try
            {
                var result = await _authService.RegisterAsync(dto);
                var userId = await _authService.GetUserIdByEmailAsync(dto.Email);

                await _auditService.LogRegistrationAsync(userId, true, ipAddress, userAgent, statusCode: 200, null, "Web", "User registered successfully");

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                await _auditService.LogRegistrationAsync(null, false, ipAddress, userAgent, statusCode: 400, null, "Web", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ipAddress = GetClientIpAddress();
            var userAgent = GetUserAgent();

            try
            {
                var result = await _authService.LoginAsync(dto);
                var userId = await _authService.GetUserIdByEmailAsync(dto.Email);

                await _auditService.LogLoginAttemptAsync(userId, true, ipAddress, userAgent, statusCode: 200, "Password", null, "Web", "Login successful");

                return Ok(new { message = "Login successfully", token = ((dynamic)result).token, refreshToken = ((dynamic)result).refreshToken });
            }
            catch (Exception ex)
            {
                var failedAttemptUserId = await _authService.GetUserIdByEmailAsync(dto.Email);
                await _auditService.LogLoginAttemptAsync(userId: failedAttemptUserId, isSuccess: false, ipAddress: ipAddress, userAgent: userAgent, statusCode: 400, authMethod: "Password", source: "Web", details: ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Scheduling Admin,Shift Supervisor")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var adminUserDto = await _authService.GetAdminUserByIdAsync(id);

            if (adminUserDto == null)
                return NotFound(new { message = "User not found." });

            return Ok(adminUserDto);
        }

        // ==========================================
        // INTERNAL HELPER ENDPOINTS (UNAUTHENTICATED)
        // ==========================================

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> UserExists(int id)
        {
            var exists = await _context.Users.AnyAsync(u => u.UserID == id);
            return Ok(exists);
        }

        [HttpPost("lookup")]
        public async Task<IActionResult> LookupUsers([FromBody] List<int> ids)
        {
            var usersDict = await _context.Users
                .Where(u => ids.Contains(u.UserID))
                .ToDictionaryAsync(u => u.UserID, u => u.Name);
            return Ok(usersDict);
        }

        [HttpGet("location/{locationId}/ids")]
        public async Task<IActionResult> GetUserIdsByLocation(int locationId)
        {
            var ids = await _context.Users
                .Where(u => u.LocationID == locationId)
                .Select(u => u.UserID)
                .ToListAsync();
            return Ok(ids);
        }

        [HttpGet("active-by-location-and-dept")]
        public async Task<IActionResult> GetActiveUsersByLocationAndDept([FromQuery] int locationId, [FromQuery] int departmentId)
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.LocationID == locationId && u.DepartmentID == departmentId && u.Status == Enums.UserStatus.Active && u.Role.roleName == "FrontLine Employee")
                .Select(u => new { u.UserID, u.EmployeeID, u.Name })
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("active-by-location")]
        public async Task<IActionResult> GetActiveUsersByLocation([FromQuery] int locationId)
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.LocationID == locationId && u.Status == Enums.UserStatus.Active && u.Role.roleName == "FrontLine Employee")
                .Select(u => new { u.UserID, u.EmployeeID, u.Name })
                .ToListAsync();
            return Ok(users);
        }
    }
}
