using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Services.DTOs;
using Services.Interfaces;

namespace ShiftMaster.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ProducesResponseType(200, Type = typeof(object))] //  Forces Swagger to stop requesting raw binary/octet-streams
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public UsersController(IAuthService authService, ApplicationDbContext context, IAuditService auditService)
        {
            _authService = authService;
            _context = context;
            _auditService = auditService;
        }

        // Helper method to get client IP address
        private string GetClientIpAddress()
        {
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
            {
                var ips = xForwardedFor.ToString().Split(',');
                return ips[0].Trim();
            }
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        // Helper method to get user agent
        private string GetUserAgent()
        {
            return HttpContext.Request.Headers["User-Agent"].ToString() ?? "Unknown";
        }

        // Endpoint: POST /api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var ipAddress = GetClientIpAddress();
            var userAgent = GetUserAgent();

            try
            {
                var result = await _authService.RegisterAsync(dto);

                // Log successful registration
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                await _auditService.LogRegistrationAsync(
                    userId: user?.UserID,
                    isSuccess: true,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    source: "Web",
                    details: "User registered successfully");

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                // Log failed registration
                await _auditService.LogRegistrationAsync(
                    userId: null,
                    isSuccess: false,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    source: "Web",
                    details: ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // Endpoint: POST /api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var ipAddress = GetClientIpAddress();
            var userAgent = GetUserAgent();

            try
            {
                var token = await _authService.LoginAsync(dto);

                // Log successful login
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                await _auditService.LogLoginAttemptAsync(
                    userId: user?.UserID,
                    isSuccess: true,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    authMethod: "Password",
                    source: "Web",
                    details: "Login successful");

                return Ok(new { message = "Login successfully", token });
            }
            catch (Exception ex)
            {
                // Log failed login
                await _auditService.LogLoginAttemptAsync(
                    userId: null,
                    isSuccess: false,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    authMethod: "Password",
                    source: "Web",
                    details: ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // Endpoint: GET /api/users/{id}
        [Authorize]
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var adminUserDto = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Include(u => u.HomeLocation)
                .Where(u => u.UserID == id)
                .Select(u => new AdminUserDto
                {
                    UserId = u.UserID,
                    EmployeeID = u.EmployeeID,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    LocationName = u.HomeLocation.LocationName,
                    RoleName = u.Role.roleName,
                    DepartmentName = u.Department.departmentName
                })
                .FirstOrDefaultAsync();

            if (adminUserDto == null)
                return NotFound(new { message = "User not found." });

            return Ok(adminUserDto);
        }
    }
}