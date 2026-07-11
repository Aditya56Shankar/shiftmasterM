using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.Employee.DTOs;
using ShiftMaster.Employee.Models;
using ShiftMaster.Employee.Services;
using ShiftMaster.Employee.Repositories;

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
            try
            {
                var entity = mapper.Map<EmployeeSkill>(request);
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
    }
}
