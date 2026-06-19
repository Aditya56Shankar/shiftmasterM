using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;
using Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillRequirementsController : ControllerBase
    {
        private readonly ISkillRequirementService _service;

        public SkillRequirementsController(ISkillRequirementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillRequirementDto>>> GetAllRequirements()
        {
            var requirements = await _service.GetAllRequirementsAsync();
            return Ok(requirements);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SkillRequirementDto>> GetRequirementById(int id)
        {
            var requirement = await _service.GetRequirementByIdAsync(id);
            if (requirement == null) return NotFound();

            return Ok(requirement);
        }

        [HttpPost]
        public async Task<ActionResult<SkillRequirementDto>> CreateRequirement(CreateSkillRequirementDto newReq)
        {
            var createdReq = await _service.CreateRequirementAsync(newReq);
            return CreatedAtAction(nameof(GetRequirementById), new { id = createdReq.SkillReqID }, createdReq);
        }
    }
}