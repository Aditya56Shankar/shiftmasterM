 using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeSkillController : ControllerBase
    {
        private readonly IEmployeeSkillRepository repository;
        private readonly IMapper mapper;

        public EmployeeSkillController(IEmployeeSkillRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill([FromBody] EmployeeSkillRequestDto request)
        {
            var entity = mapper.Map<EmployeeSkill>(request);
            var saved = await repository.AddEmployeeSkillAsync(entity);
            var response = mapper.Map<EmployeeSkillResponseDto>(saved);
            return Ok(response);
        }
    }
}
