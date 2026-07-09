using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;
using shiftmaster.models;
using System.Security.Claims;

[ApiController]
[Route("api/attendance")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _repo;
    private readonly IMapper _mapper;

    public AttendanceController(IAttendanceService repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Roles = "FrontLine Employee")]
    public async Task<IActionResult> RecordAttendance([FromBody] CreateAttendanceDto dto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized("Invalid user.");
        }

        try
        {
            var entity = _mapper.Map<AttendanceRecord>(dto);
            entity.UserID = userId;

            var result = await _repo.CreateAttendanceAsync(entity);
            var response = _mapper.Map<AttendanceDtoResponse>(result);

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
    public async Task<IActionResult> GetMyAttendance([FromQuery] string? date)
    {
        var validationResult = ValidateDateParameter(date, "date");
        if (validationResult != null!)
        {
            return validationResult;
        }

        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized("Invalid user.");
        }

        var parsedDate = DateTime.ParseExact(date!, "yyyy-MM-dd", null);
        var result = await _repo.GetAttendanceForUserDateAsync(userId, parsedDate);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AttendanceDtoResponse>(result));
    }

    [HttpGet("week")]
    [Authorize(Roles = "FrontLine Employee")]
    public async Task<IActionResult> GetAttendanceWeek([FromQuery] string? weekStart)
    {
        var validationResult = ValidateDateParameter(weekStart, "weekStart");
        if (validationResult != null!)
        {
            return validationResult;
        }

        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized("Invalid user.");
        }

        var parsedWeekStart = DateTime.ParseExact(weekStart!, "yyyy-MM-dd", null);
        var result = await _repo.GetAttendanceForUserWeekAsync(userId, parsedWeekStart);
        return Ok(_mapper.Map<List<AttendanceDtoResponse>>(result));
    }

    [HttpGet("location/{locationId}")]
    [Authorize(Roles = "Shift Supervisor")]
    public async Task<IActionResult> GetAttendanceByLocation(int locationId, [FromQuery] string? date)
    {
        if (locationId <= 0)
        {
            return BadRequest("Invalid locationId.");
        }

        var validationResult = ValidateDateParameter(date, "date");
        if (validationResult != null!)
        {
            return validationResult;
        }

        try
        {
            var parsedDate = DateTime.ParseExact(date!, "yyyy-MM-dd", null);
            var result = await _repo.GetAttendanceByLocationDateAsync(locationId, parsedDate);
            return Ok(_mapper.Map<List<AttendanceDtoResponse>>(result));
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("flagged")]
    [Authorize(Roles = "Shift Supervisor")]
    public async Task<IActionResult> GetFlaggedAttendance([FromQuery] int locationId, [FromQuery] string? date)
    {
        if (locationId <= 0)
        {
            return BadRequest("Invalid locationId.");
        }

        var validationResult = ValidateDateParameter(date, "date");
        if (validationResult != null!)
        {
            return validationResult;
        }

        try
        {
            var parsedDate = DateTime.ParseExact(date!, "yyyy-MM-dd", null);
            var result = await _repo.GetFlaggedAttendanceByLocationDateAsync(locationId, parsedDate);
            return Ok(_mapper.Map<List<AttendanceDtoResponse>>(result));
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Shift Supervisor")]
    public async Task<IActionResult> GetAttendanceById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid attendance id.");
        }

        var result = await _repo.GetAttendanceByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AttendanceDtoResponse>(result));
    }

    [HttpPut("{id}/excuse")]
    [Authorize(Roles = "Shift Supervisor")]
    public async Task<IActionResult> ExcuseAttendance(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid attendance id.");
        }

        try
        {
            var result = await _repo.ExcuseAttendanceAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AttendanceDtoResponse>(result));
        }
        catch (InvalidWorkflowStateException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/correct")]
    [Authorize(Roles = "Shift Supervisor")]
    public async Task<IActionResult> CorrectAttendance(int id, [FromBody] UpdateAttendanceDto dto)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid attendance id.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _repo.CorrectAttendanceAsync(id, dto.ClockIn, dto.ClockOut, dto.BreakMinutesTaken);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AttendanceDtoResponse>(result));
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

        private IActionResult ValidateDateParameter(string? dateString, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return BadRequest($"Query parameter '{parameterName}' is required. Use ISO 8601 format: YYYY-MM-DD");
            }

            const string iso8601Format = "yyyy-MM-dd";
            if (!DateTime.TryParseExact(dateString, iso8601Format, null, System.Globalization.DateTimeStyles.None, out _))
            {
                return BadRequest($"Invalid {parameterName} format. Expected ISO 8601 format: YYYY-MM-DD. Received: {dateString}");
            }

            return null!;
        }
    }
