using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShiftMaster.SchedulingService.DTOs;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Enums;
using ShiftMaster.SchedulingService.Services;
using ShiftMaster.SchedulingService.Repositories;
using ShiftMaster.SchedulingService.Exceptions;

namespace ShiftMaster.SchedulingService.Controllers
{
    [Route("api/shiftassignment")]
    [ApiController]
    public class ShiftAssignmentController : ControllerBase
    {
        private readonly IRosterValidationService _validationService;
        private readonly IShiftRepository _shiftRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ShiftAssignmentController> _logger;

        public ShiftAssignmentController(
            IRosterValidationService validationService,
            IShiftRepository shiftRepo,
            ILogger<ShiftAssignmentController> logger,
            IMapper mapper)
        {
            _validationService = validationService;
            _shiftRepo = shiftRepo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> AssignShift(CreateAssignmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool duplicateExists = await _shiftRepo.ShiftExistsAsync(
                    dto.UserID,
                    dto.AssignedDate,
                    dto.StartTime,
                    dto.EndTime);

                if (duplicateExists)
                {
                    return BadRequest(new
                    {
                        Message = $"Employee {dto.UserID} already has a shift assigned on " +
                                  $"{dto.AssignedDate:yyyy-MM-dd} from {dto.StartTime} to {dto.EndTime}."
                    });
                }

                var assignment = _mapper.Map<ShiftAssignment>(dto);

                await _shiftRepo.AddAsync(assignment);
                await _shiftRepo.SaveAsync();

                try
                {
                    await _validationService.ValidateAssignmentConstraintsAsync(assignment.AssignmentID);
                }
                catch (InvalidWorkflowStateException ex)
                {
                    _logger.LogWarning($"Validation warning for assignment {assignment.AssignmentID}: {ex.Message}");
                    throw;
                }

                var updatedAssignment = await _shiftRepo.GetShiftWithDetailsAsync(assignment.AssignmentID);
                var response = _mapper.Map<AssignmentResponseDto>(updatedAssignment);

                return Ok(response);
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        // ==========================================
        // INTERNAL HELPER ENDPOINTS (UNAUTHENTICATED)
        // ==========================================

        [HttpGet("internal/assignment/{assignmentId}")]
        public async Task<IActionResult> GetAssignmentInternal(int assignmentId)
        {
            var assignment = await _shiftRepo.GetShiftWithDetailsAsync(assignmentId);
            if (assignment == null)
                return NotFound();

            var durationHours = assignment.Pattern != null 
                ? assignment.Pattern.DurationHours 
                : (decimal)(assignment.EndTime - assignment.StartTime).TotalHours;

            if (durationHours < 0) durationHours += 24;

            var response = new EmployeeShiftDto
            {
                AssignmentId = assignment.AssignmentID,
                AssignedDate = assignment.AssignedDate,
                StartTime = assignment.StartTime,
                EndTime = assignment.EndTime,
                Status = assignment.Status.ToString(),
                UserID = assignment.UserID,
                LocationID = assignment.Roster != null ? assignment.Roster.LocationID : 0,
                DurationHours = durationHours
            };

            return Ok(response);
        }

        [HttpGet("internal/user/{userId}")]
        public async Task<IActionResult> GetShiftsByUserIdInternal(int userId)
        {
            var shifts = await _shiftRepo.GetByUserIdAsync(userId);
            var response = shifts.Select(s => new EmployeeShiftDto
            {
                AssignmentId = s.AssignmentID,
                AssignedDate = s.AssignedDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status.ToString(),
                UserID = s.UserID,
                LocationID = s.Roster != null ? s.Roster.LocationID : 0,
                DurationHours = s.Pattern != null ? s.Pattern.DurationHours : (decimal)(s.EndTime - s.StartTime).TotalHours
            }).ToList();

            return Ok(response);
        }
    }

    // Helper model mapping just for the internal endpoint
    public class EmployeeShiftDto
    {
        public int AssignmentId { get; set; }
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = null!;
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public decimal DurationHours { get; set; }
    }
}
