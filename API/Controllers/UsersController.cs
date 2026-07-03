using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;

namespace ShiftMaster.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ProducesResponseType(200, Type = typeof(object))]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuditService _auditService;

        public UsersController(IAuthService authService, IAuditService auditService)
        {
            _authService = authService;
            _auditService = auditService;
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

                // Delegated to the user service
                var userId = await _authService.GetUserIdByEmailAsync(dto.Email);

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
        public async Task<IActionResult> Login([FromBody] LoginDto dto)  // dto taked value from body of the request
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ipAddress = GetClientIpAddress();
            var userAgent = GetUserAgent();

            try
            {
                var token = await _authService.LoginAsync(dto);

                // Delegated to the user service
                var userId = await _authService.GetUserIdByEmailAsync(dto.Email);

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
        [Authorize(Roles = "Scheduling Admin,Shift Supervisor")]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Delegated to the user service
            var adminUserDto = await _authService.GetAdminUserByIdAsync(id);

            if (adminUserDto == null)
                return NotFound(new { message = "User not found." });

            return Ok(adminUserDto);
        }
    }
}