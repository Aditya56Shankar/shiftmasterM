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


    [Authorize(Roles = "Supervisor")]
    [HttpPut("{id}/Supervisor")]
    public async Task<IActionResult> SendToPayroll(int id)
    {
        var userId = int.Parse(User.FindFirst("nameid")?.Value);

        var result = await _repo.UpdateTimesheetStatusAsync(
            id,
            TimesheetStatus.SentToPayroll,
            userId
        );

        if (result == null)
            return NotFound();

        return Ok(result);
    }


    //  PUT → Approve (Supervisor)
    [Authorize(Roles = "Payroll")]
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveTimesheet(int id)
    {
        var userId = int.Parse(User.FindFirst("nameid")?.Value);

        var result = await _repo.UpdateTimesheetStatusAsync(
            id,
            TimesheetStatus.Approved,
            userId
        );

        if (result == null)
            return NotFound();

        return Ok(result);
    }





}