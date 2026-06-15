using AutoMapper;
using Data.Context;
using Domain.Enums;
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
        private readonly ApplicationDbContext _context;

        private readonly IWeeklyRosterRepository repository;
        private readonly IMapper mapper;

        public RostersController(IWeeklyRosterRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> CreateRoster([FromBody] CreateRosterDto dto)
        {
            var res = mapper.Map<WeeklyRoster>(dto);
            await repository.AddAsync(res);

            var roster = new WeeklyRoster
            {
                LocationID = dto.LocationID,
                DepartmentID = dto.DepartmentID,
                WeekStartDate = dto.WeekStartDate.Date,
                WeekEndDate = dto.WeekStartDate.Date.AddDays(6),
                CreatedByID = dto.CreatedByID,
                Status = RosterStatus.Draft
            };

            return Ok(mapper.Map<RosterResponseDto>(res));
        }

        // HLD Endpoint: GET /api/rosters/{locationId}/{week} (Action: Get roster)
        // Note: '{week}' represents the week start date string (e.g., "2026-06-15")
        //[HttpGet]
        //[Route("{locationId:int}/{week}")]
        //public async Task<IActionResult> GetRoster(int locationId, string week)
        //{
        //    if (!DateTime.TryParse(week, out DateTime parsedDate))
        //    {
        //        return BadRequest("Invalid date format. Please use YYYY-MM-DD.");
        //    }

        //    var roster = await _context.WeeklyRosters
        //        .Include(r => r.ShiftAssignments)
        //        .Include(r => r.Violations)
        //        .FirstOrDefaultAsync(r => r.LocationID == locationId && r.WeekStartDate.Date == parsedDate.Date);

        //    if (roster == null) return NotFound("No roster found for this location and week.");

        //    // Manually gather employee names to safely return the complete board grid matrix
        //    var assignmentList = new List<SupervisorAssignmentViewDto>();
        //    foreach (var sa in roster.ShiftAssignments)
        //    {
        //        var empName = await _context.Users
        //            .Where(u => u.UserID == sa.UserID)
        //            .Select(u => u.Name)
        //            .FirstOrDefaultAsync() ?? "Unknown Employee";

        //        assignmentList.Add(new SupervisorAssignmentViewDto
        //        {
        //            AssignmentID = sa.AssignmentID,
        //            UserID = sa.UserID,
        //            EmployeeName = empName,
        //            AssignedDate = sa.AssignedDate,
        //            StartTime = sa.StartTime,
        //            EndTime = sa.EndTime,
        //            Role = sa.Role,
        //            Status = sa.Status.ToString()
        //        });
        //    }

        //    var response = new SupervisorRosterResponseDto
        //    {
        //        RosterID = roster.RosterID,
        //        LocationID = roster.LocationID ?? 0,
        //        DepartmentID = roster.DepartmentID ?? 0,
        //        WeekStartDate = roster.WeekStartDate,
        //        WeekEndDate = roster.WeekEndDate,
        //        Status = roster.Status.ToString(),
        //        CreatedByID = roster.CreatedByID,
        //        PublishedDate = roster.PublishedDate,
        //        ShiftAssignments = assignmentList,
        //        Violations = roster.Violations.Select(v => new ViolationViewDto
        //        {
        //            ViolationID = v.ViolationID,
        //            UserID = v.UserID ?? 0,
        //            ViolationType = v.ViolationType.ToString(),
        //            Severity = v.Severity.ToString(),
        //            Status = v.Status.ToString()
        //        }).ToList()
        //    };

        //    return Ok(response);

           
    }
}

