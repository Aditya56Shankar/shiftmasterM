using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

[ApiController]
[Route("api/attendance")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceRepository _repo;
    private readonly IMapper _mapper;

    public AttendanceController(IAttendanceRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // ✅ POST: /api/attendance
    [HttpPost]
    public async Task<IActionResult> RecordAttendance([FromBody] CreateAttendanceDto dto)
    {
        // ✅ Map DTO → Entity
        var entity = _mapper.Map<AttendanceRecord>(dto);

        // ✅ Save
        var result = await _repo.CreateAttendanceAsync(entity);

        // ✅ Map Entity → Response DTO
        var response = _mapper.Map<AttendanceDtoResponse>(result);

        return Ok(response);
    }
}