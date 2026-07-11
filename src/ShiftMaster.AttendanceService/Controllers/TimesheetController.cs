using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.AttendanceService.Applications.Interfaces;
using ShiftMaster.AttendanceService.Domain.Enums;
using ShiftMaster.AttendanceService.Applications.Exceptions;
using ShiftMaster.AttendanceService.Applications.DTOs;

namespace ShiftMaster.AttendanceService.Controllers
{
    [ApiController]
    [Route("api/timesheets")]
    public class TimesheetController : ControllerBase
    {
        private readonly IAttendanceService _repo;
        private readonly IMapper _mapper;

        public TimesheetController(IAttendanceService repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> SubmitTimesheet([FromBody] CreateTimesheetDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            try
            {
                var result = await _repo.CreateTimesheetAsync(userId, dto.WeekStartDate);
                var response = _mapper.Map<TimesheetDtoResponse>(result);

                return Ok(response);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my")]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> GetMyTimesheet([FromQuery] DateTime weekStart)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            try
            {
                var result = await _repo.GetTimesheetAsync(userId, weekStart);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<TimesheetDtoResponse>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Shift Supervisor,HR,PayRoll Executive")]
        public async Task<IActionResult> GetTimesheetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid timesheet id.");
            }

            try
            {
                var result = await _repo.GetTimesheetByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<TimesheetDtoResponse>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pending-supervisor")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> GetPendingSupervisorTimesheets([FromQuery] int locationId)
        {
            if (locationId <= 0)
            {
                return BadRequest("Invalid locationId.");
            }

            try
            {
                var result = await _repo.GetPendingSupervisorTimesheetsAsync(locationId);
                return Ok(_mapper.Map<List<TimesheetDtoResponse>>(result));
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/supervisor-approve")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> SupervisorApproveTimesheet(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid timesheet id.");
            }

            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            try
            {
                var result = await _repo.UpdateTimesheetStatusAsync(id, TimesheetStatus.SupervisorApproved, userId);

                if (result == null)
                    return NotFound();

                return Ok(_mapper.Map<TimesheetDtoResponse>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Shift Supervisor,HR")]
        public async Task<IActionResult> RejectTimesheet(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid timesheet id.");
            }

            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            try
            {
                var result = await _repo.UpdateTimesheetStatusAsync(id, TimesheetStatus.Submitted, userId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<TimesheetDtoResponse>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pending-hr")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetPendingHrTimesheets([FromQuery] int locationId)
        {
            if (locationId <= 0)
            {
                return BadRequest("Invalid locationId.");
            }

            try
            {
                var result = await _repo.GetPendingHrTimesheetsAsync(locationId);
                return Ok(_mapper.Map<List<TimesheetDtoResponse>>(result));
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/hr-approve")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> HrApproveTimesheet(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid timesheet id.");
            }

            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            try
            {
                var result = await _repo.UpdateTimesheetStatusAsync(id, TimesheetStatus.HrApproved, userId);

                if (result == null)
                    return NotFound();

                return Ok(_mapper.Map<TimesheetDtoResponse>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("approved-for-payroll")]
        [Authorize(Roles = "PayRoll Executive")]
        public async Task<IActionResult> GetApprovedForPayroll([FromQuery] DateTime weekStart)
        {
            try
            {
                var result = await _repo.GetApprovedTimesheetsForPayrollAsync(weekStart);
                return Ok(_mapper.Map<List<TimesheetDtoResponse>>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "PayRoll Executive")]
        [HttpPut("{id}/payroll")]
        public async Task<IActionResult> SendToPayroll(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid timesheet id.");
            }

            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            try
            {
                var result = await _repo.UpdateTimesheetStatusAsync(id, TimesheetStatus.SentToPayroll, userId);

                if (result == null)
                    return NotFound();

                return Ok(_mapper.Map<TimesheetDtoResponse>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool TryGetCurrentUserId(out int actorUserId)
        {
            var userIdClaim = User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out actorUserId);
        }
    }
}
