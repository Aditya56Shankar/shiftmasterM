using AutoMapper;
using Data.Context;
using Domain.Enums;
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

            

                LocationID = dto.LocationID,
                DepartmentID = dto.DepartmentID,
                WeekStartDate = dto.WeekStartDate.Date,
                WeekEndDate = dto.WeekStartDate.Date.AddDays(6),
                CreatedByID = dto.CreatedByID,
                Status = RosterStatus.Draft
            };

            return Ok(mapper.Map<RosterResponseDto>(res));
        }

        //HLD Endpoint: GET /api/rosters/{locationId}/{week} (Action: Get roster)
        //Note: '{week}' represents the week start date string (e.g., "2026-06-15")
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


    }
}

