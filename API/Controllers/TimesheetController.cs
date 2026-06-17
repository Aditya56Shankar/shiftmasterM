using AutoMapper;
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
}