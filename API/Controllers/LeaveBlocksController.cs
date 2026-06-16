using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveBlocksController : ControllerBase
    {
        private readonly ILeaveBlockRepository repository;
        private readonly IMapper mapper;

        public LeaveBlocksController(ILeaveBlockRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveBlockRequestDto leave)
        {

            if (leave == null) {
                return BadRequest("enter correctly");
            }
            var res = mapper.Map<LeaveBlock>(leave);
            var saved = await repository.AddLeaveBlockAsync(res);
            var response = mapper.Map<LeaveBlockResponseDto>(saved);

            return Ok(response);
        }
    }
}
