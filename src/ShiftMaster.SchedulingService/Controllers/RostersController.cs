using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.SchedulingService.Clients;
using ShiftMaster.SchedulingService.Application.Interfaces;
using ShiftMaster.SchedulingService.Application.Exceptions;
using ShiftMaster.SchedulingService.Application.DTOs;
using ShiftMaster.SchedulingService.Domain.Models;

namespace ShiftMaster.SchedulingService.Controllers
{
    [Route("api/rosters")]
    [ApiController]
    public class RostersController : ControllerBase
    {
        private readonly IWeeklyRosterService service;
        private readonly IMapper mapper;
        private readonly IEmployeeClient _employeeClient;
        private readonly IIdentityClient _identityClient;

        public RostersController(IWeeklyRosterService service, IMapper mapper, IEmployeeClient employeeClient, IIdentityClient identityClient)
        {
            this.service = service;
            this.mapper = mapper;
            this._employeeClient = employeeClient;
            this._identityClient = identityClient;
        }

        
        [HttpPost]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> CreateRoster([FromBody] CreateRosterDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid roster data payload.");

            try
            {
                // 1. Enforce validations against Identity Microservice
                if (!await _identityClient.LocationExistsAsync(dto.LocationID))
                    return BadRequest(new { Message = $"Location with ID {dto.LocationID} does not exist." });

                if (!await _identityClient.DepartmentExistsAsync(dto.DepartmentID))
                    return BadRequest(new { Message = $"Department with ID {dto.DepartmentID} does not exist." });

                // 2. Extract and validate user ID using literal "nameid" from token context
                var userIdClaim = User.FindFirst("nameid")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int createdById))
                    return Unauthorized(new { Message = "Valid User ID not found in token context." });

                if (!await _identityClient.UserExistsAsync(createdById))
                    return BadRequest(new { Message = $"Roster creator with User ID {createdById} does not exist." });

                // 3. Map the DTO and inject the Token UserID directly during instantiation
                var entity = mapper.Map<WeeklyRoster>(dto, opts =>
                {
                    opts.Items["TokenUserId"] = createdById;
                });

                // 4. Persist via the service layer
                var result = await service.AddAsync(entity);
                return Ok(mapper.Map<RosterResponseDto>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ResourceNotFoundException ex)
            {
                return StatusCode(404, new { Message = "Resource not found", Error = ex.Message });
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
                // FIXED: Updated claim target lookup to use "nameid" key matching your token configuration
                var userIdClaim = User.FindFirst("nameid")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { Message = "User ID not found in token context." });

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