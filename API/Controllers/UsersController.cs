using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Implementation;
using Services.Interfaces;

namespace ShiftMaster.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ProducesResponseType(200, Type = typeof(object))]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;

        public UsersController(IAuthService authService, ApplicationDbContext context, IAuditService auditService, IUserService userService)
        {
            _authService = authService;
            _context = context;
            _auditService = auditService;
            _userService = userService;
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
            var ipAddress = GetClientIpAddress();
            var userAgent = GetUserAgent();

            try
            {
                var result = await _authService.RegisterAsync(dto);

                // Optimized: Only select the UserID instead of tracking the full entity
                var userId = await _context.Users
                    .Where(u => u.Email == dto.Email)
                    .Select(u => (int?)u.UserID)
                    .FirstOrDefaultAsync();

                await _auditService.LogRegistrationAsync(userId, true, ipAddress, userAgent, "Web", "User registered successfully");

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                await _auditService.LogRegistrationAsync(null, false, ipAddress, userAgent, "Web", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var ipAddress = GetClientIpAddress();
            var userAgent = GetUserAgent();

            try
            {
                var token = await _authService.LoginAsync(dto);

                // Optimized: Only select the UserID instead of tracking the full entity
                var userId = await _context.Users
                    .Where(u => u.Email == dto.Email)
                    .Select(u => (int?)u.UserID)
                    .FirstOrDefaultAsync();

                await _auditService.LogLoginAttemptAsync(userId, true, ipAddress, userAgent, "Password", "Web", "Login successful");

                return Ok(new { message = "Login successfully", token });
            }
            catch (Exception ex)
            {
                await _auditService.LogLoginAttemptAsync(null, false, ipAddress, userAgent, "Password", "Web", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var adminUserDto = await _context.Users
                .AsNoTracking() // Optimized: Faster read-only query
                .Where(u => u.UserID == id)
                .Select(u => new AdminUserDto
                {
                    UserId = u.UserID,
                    EmployeeID = u.EmployeeID,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    LocationName = u.HomeLocation.LocationName,
                    RoleName = u.Role.roleName.ToString(),
                    DepartmentName = u.Department.departmentName
                })
                .FirstOrDefaultAsync();

            if (adminUserDto == null)
                return NotFound(new { message = "User not found." });

            return Ok(adminUserDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto dto)
        {
            var updated = await _userService.UpdateUserAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}