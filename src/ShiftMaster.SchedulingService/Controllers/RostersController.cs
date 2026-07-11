using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.SchedulingService.DTOs;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Services;
using ShiftMaster.SchedulingService.Clients;
using ShiftMaster.SchedulingService.Exceptions;

namespace ShiftMaster.SchedulingService.Controllers
{
    [Route("api/rosters")]
    [ApiController]
    public class RostersController : ControllerBase
    {
        private readonly IWeeklyRosterService service;
        private readonly IMapper mapper;
        private readonly IEmployeeClient _employeeClient;

        public RostersController(IWeeklyRosterService service, IMapper mapper, IEmployeeClient employeeClient)
        {
            this.service = service;
            this.mapper = mapper;
            this._employeeClient = employeeClient;
        }

        [HttpPost]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> CreateRoster([FromBody] CreateRosterDto dto)
        {
            try
            {
                var entity = mapper.Map<WeeklyRoster>(dto);
                var result = await service.AddAsync(entity);
                return Ok(mapper.Map<RosterResponseDto>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ResourceNotFoundException ex)
            {
                return StatusCode(404, new { Message = "resource not found", Error = ex.Message });
            }
        }

        [HttpGet("{locationId:int}/{week}")]
        [Authorize(Roles = "Shift Supervisor,Admin")]
        public async Task<IActionResult> GetRoster(int locationId, string week)
        {
            try
            {
                if (!DateTime.TryParse(week, out DateTime parsedDate))
                    return BadRequest("Invalid date format. Use YYYY-MM-DD");

                var response = await service.GetRosterAsync(locationId, parsedDate);

                if (response == null)
                    return NotFound("No roster found");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [Authorize(Roles = "Shift Supervisor")]
        [HttpGet("{locationId:int}/employees/{date}")]
        public async Task<IActionResult> GetEmployeesFull(int locationId, string date)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                    return BadRequest("Invalid date format. Use YYYY-MM-DD");

                var result = await _employeeClient.GetEmployeesFullDataAsync(locationId, parsedDate);

                if (result == null || !result.Any())
                    return NotFound("No employees found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> UpdateRosterStatus([FromQuery] int id, [FromQuery] string action)
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
