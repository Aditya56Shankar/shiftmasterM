using Data.Context;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftAssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRosterValidationService _validationService;

        public ShiftAssignmentController(ApplicationDbContext context, IRosterValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // HLD Endpoint: POST /api/shiftassignments (Action: Assign employee to shift)
        [HttpPost]
        public async Task<IActionResult> AssignShift([FromBody] CreateAssignmentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var rosterExists = await _context.WeeklyRosters.AnyAsync(r => r.RosterID == dto.RosterID);
            if (!rosterExists) return NotFound($"Parent Weekly Roster with ID {dto.RosterID} does not exist.");

            var assignment = new ShiftAssignment
            {
                RosterID = dto.RosterID,
                UserID = dto.UserID,
                ShiftPatternID = dto.ShiftPatternID,
                AssignedDate = dto.AssignedDate.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Role = dto.Role,
                Status = ShiftAssignmentStatus.Assigned
            };

            _context.ShiftAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            // Automatically run compliance evaluations behind the scenes
            await _validationService.ValidateAssignmentConstraintsAsync(assignment.AssignmentID);

            return Ok(assignment);
        }
    }
}
