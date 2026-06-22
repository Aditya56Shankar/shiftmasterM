using System.Security.Claims;
using AutoMapper;
using Data.Context;
using Domain.Enums;
using Domain.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using Services.Mapper;
using shiftmaster.models;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RostersController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        private readonly IWeeklyRosterRepository repository;
        private readonly IMapper mapper;

        public RostersController(IWeeklyRosterRepository repository, IMapper mapper, ApplicationDbContext db)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.db = db;
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> CreateRoster([FromBody] CreateRosterDto dto)
        {
            var res = mapper.Map<WeeklyRoster>(dto);
            await repository.AddAsync(res);

            return Ok(mapper.Map<RosterResponseDto>(res));
        }

        //HLD Endpoint: GET /api/rosters/{locationId}/{week} (Action: Get roster)//Note: '{week}' represents the week start date string (e.g., "2026-06-15")
        [HttpGet]
        [Route("{locationId:int}/{week}")]
        [Authorize(Roles = "Supervisor,Admin")]
        public async Task<IActionResult> GetRoster(int locationId, string week)
        {

            if (!DateTime.TryParse(week, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Please use YYYY-MM-DD.");
            }

            var response = await repository.GetRosterAsync(locationId, parsedDate);

            if (response == null)
            {
                return NotFound("No roster found for this location and week.");
            }

            return Ok(response);

        }

        [Authorize(Roles = "Supervisor")]
        [HttpPut("{id}/update-status")]
        public async Task<IActionResult> UpdateRosterStatus(int id, [FromQuery] string action)
        {
            var roster = await db.WeeklyRosters.FindAsync(id);

            if (roster == null)
                return NotFound("Roster not found");

            var userIdClaim = User.FindFirst("nameid")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            int adminId = int.Parse(userIdClaim);

            action = action?.ToLower();

            if (action == "publish")
            {
                roster.Status = RosterStatus.Published;
            }
            else if (action == "amend")
            {
                roster.Status = RosterStatus.Amended;
            }
            else
            {
                return BadRequest("Invalid action. Use 'publish' or 'amend'");
            }

            roster.ApprovedByUserID = adminId;

            await db.SaveChangesAsync();

            return Ok(new
            {
                message = $"Roster {roster.Status} successfully",
                Status = roster.Status,
                UpdatedBy = adminId
            });
        }


    }
}

