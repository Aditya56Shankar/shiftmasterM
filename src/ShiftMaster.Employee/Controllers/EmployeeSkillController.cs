using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.Employee.Application.DTOs;
using ShiftMaster.Employee.Application.Interfaces;
using ShiftMaster.Employee.Domain.Models;
using ShiftMaster.Employee.Infrastructure.Repositories;

namespace ShiftMaster.Employee.Controllers
{
    [ApiController]
    [Route("api/employeeskill")]
    public class EmployeeSkillController : ControllerBase
    {
        private readonly IEmployeeSkillService service;
        private readonly IMapper mapper;

        public EmployeeSkillController(IEmployeeSkillService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> AddSkill([FromBody] EmployeeSkillRequestDto request)
        {
            if (request == null)
                return BadRequest("Invalid skill request data.");

            // 1. Extract the actual User ID securely tied to the JWT token
            if (!TryGetCurrentUserId(out var actorUserId))
            {
                return Unauthorized("Invalid or missing user identity in token.");
            }

            try
            {
                // 2. Pass the extracted actorUserId directly to the AutoMapper context items.
                var entity = mapper.Map<EmployeeSkill>(request, opts =>
                {
                    opts.Items["TokenUserId"] = actorUserId;
                });

                // 3. Process through the service layer (which cross-checks against IdentityDB)
                var saved = await service.AddEmployeeSkillAsync(entity);
                var response = mapper.Map<EmployeeSkillResponseDto>(saved);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        // ==========================================
        // INTERNAL HELPER ENDPOINTS (UNAUTHENTICATED)
        // ==========================================

        [HttpGet("internal/{userId}/skills")]
        public async Task<IActionResult> GetEmployeeSkillsInternal(int userId, [FromServices] ISkillRepository skillRepo)
        {
            var skills = await skillRepo.GetEmployeeSkillsAsync(userId);
            return Ok(skills);
        }

        // ==========================================
        // PRIVATE UTILITY METHODS
        // ==========================================
        private bool TryGetCurrentUserId(out int actorUserId)
        {
            var userIdClaim = User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out actorUserId);
        }
    }
}