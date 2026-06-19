using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;

[ApiController]
[Route("api/timesheets")]
public class TimesheetController : ControllerBase
{
    private readonly IAttendanceRepository _repo;
    private readonly IMapper _mapper;

    public TimesheetController(IAttendanceRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // ✅ POST: /api/timesheets
    [HttpPost]
    public async Task<IActionResult> SubmitTimesheet([FromBody] CreateTimesheetDto dto)
    {
        // ✅ Generate Timesheet
        var result = await _repo.CreateTimesheetAsync(dto.UserID, dto.WeekStartDate);

        // ✅ Map Entity → Response DTO
        var response = _mapper.Map<TimesheetDtoResponse>(result);

        return Ok(response);
    }

    //  PUT → Approve (Supervisor)

    [Authorize(Roles = "ShiftSupervisor")]
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveTimesheet(int id)
    {
        var result = await _repo.UpdateTimesheetStatusAsync(
            id,
            TimesheetStatus.Approved
            );

        if (result == null)
            return NotFound();

        var response = _mapper.Map<TimesheetDtoResponse>(result);
        return Ok(response);
    }


    [Authorize(Roles = "PayrollExecutive")]
    [HttpPut("{id}/payroll")]
    public async Task<IActionResult> SendToPayroll(int id)
    {
        var result = await _repo.UpdateTimesheetStatusAsync(
            id,
            TimesheetStatus.SentToPayroll
            );

        if (result == null)
            return NotFound();

        var response = _mapper.Map<TimesheetDtoResponse>(result);
        return Ok(response);
    }


}