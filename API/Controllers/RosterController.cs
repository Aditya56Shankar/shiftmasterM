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
        public async Task<IActionResult> CreateRoster([FromBody] CreateRosterDto dto)
        {
            var res = mapper.Map<WeeklyRoster>(dto);
            await repository.AddAsync(res);

            return Ok(mapper.Map<RosterResponseDto>(res));
        }

        //HLD Endpoint: GET /api/rosters/{locationId}/{week} (Action: Get roster)//Note: '{week}' represents the week start date string (e.g., "2026-06-15")
        [HttpGet]
        [Route("{locationId:int}/{week}")]
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

        [Authorize(Roles = "SchedulingAdmin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRoster(int id)
        {
            var roster = await db.WeeklyRosters.FindAsync(id);

            if (roster == null)
                return NotFound("Roster not found");

            if (roster.ApprovedByUserID != null)
                return BadRequest("Roster already approved");

            // ✅ Get User ID from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            roster.ApprovedByUserID = int.Parse(userIdClaim);

            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Roster approved by Scheduling Admin",
                ApprovedBy = roster.ApprovedByUserID,
            });
        }

    }
}

