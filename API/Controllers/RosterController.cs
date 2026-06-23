using System.Security.Claims;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RostersController : ControllerBase
    {
        private readonly IWeeklyRosterService service;
        private readonly IMapper mapper;

        public RostersController(IWeeklyRosterService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        // ✅ CREATE
        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> CreateRoster([FromBody] CreateRosterDto dto)
        {
            var entity = mapper.Map<WeeklyRoster>(dto);

            var result = await service.AddAsync(entity);

            return Ok(mapper.Map<RosterResponseDto>(result));
        }

        // ✅ GET
        [HttpGet("{locationId:int}/{week}")]
        [Authorize(Roles = "Supervisor,Admin")]
        public async Task<IActionResult> GetRoster(int locationId, string week)
        {
            if (!DateTime.TryParse(week, out DateTime parsedDate))
                return BadRequest("Invalid date format. Use YYYY-MM-DD");

            var response = await service.GetRosterAsync(locationId, parsedDate);

            if (response == null)
                return NotFound("No roster found");

            return Ok(response);
        }

        // ✅ UPDATE STATUS
        [HttpPut("{id}/update-status")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> UpdateRosterStatus(int id, [FromQuery] string action)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found");

                int userId = int.Parse(userIdClaim);

                var result = await service.UpdateRosterStatusAsync(id, action, userId);

                if (!result)
                    return NotFound("Roster not found");

                return Ok(new
                {
                    message = $"Roster {action} successfully",
                    UpdatedBy = userId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}