using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.Employee.Application.DTOs;
using ShiftMaster.Employee.Application.Interfaces;
using ShiftMaster.Employee.Domain.Enums;
using ShiftMaster.Employee.Domain.Models;

namespace ShiftMaster.Employee.Controllers
{
    [Route("api/availability")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAvailabilityService service;

        public AvailabilityController(IMapper mapper, IAvailabilityService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        [HttpPost]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> Availability([FromBody] AvailabilityRequestDto avail)
        {
            if (avail == null)
                return BadRequest("Invalid availability data request.");

            // 1. Extract the actual User ID securely tied to the JWT token
            if (!TryGetCurrentUserId(out var actorUserId))
            {
                return Unauthorized("Invalid or missing user identity in token.");
            }

            try
            {
                // 2. Pass the extracted actorUserId directly to the AutoMapper context items.
                // This maps the DTO properties and sets entity.UserID via the mapping profile.
                var entity = mapper.Map<AvailabilitySubmission>(avail, opts =>
                {
                    opts.Items["TokenUserId"] = actorUserId;
                });

                // 3. Process through the service layer (which cross-checks against IdentityDB)
                var result = await service.AddAvailableAsync(entity);
                return Ok(mapper.Map<AvailabilityResponseDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "An error occurred while processing your availability request.",
                    Error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> UpdateAvailabilityStatus(int id, [FromQuery] AvailabilityStatus status)
        {
            try
            {
                var updated = await service.UpdateAvailabilityStatusAsync(id, status);

                if (!updated)
                    return NotFound("Availability not found");

                return Ok(new
                {
                    message = "Availability status updated successfully",
                    AvailabilityID = id,
                    UpdatedStatus = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> GetMySchedule()
        {
            try
            {
                if (!TryGetCurrentUserId(out var userId))
                    return Unauthorized();

                var result = await service.GetMyScheduleAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred.",
                    Error = ex.Message
                });
            }
        }

        // ==========================================
        // INTERNAL HELPER ENDPOINTS (UNAUTHENTICATED)
        // ==========================================

        [HttpGet("internal/{userId}/availability")]
        public async Task<IActionResult> GetAvailabilityInternal(int userId, [FromQuery] DateTime targetDate)
        {
            var avail = await service.GetAvailabilityAsync(userId, targetDate);
            if (avail == null) return NotFound();
            return Ok(avail);
        }

        [HttpGet("internal/{userId}/availability/confirmed")]
        public async Task<IActionResult> IsConfirmedInternal(int userId, [FromQuery] DateTime date)
        {
            var isConfirmed = await service.IsConfirmedAsync(userId, date);
            return Ok(isConfirmed);
        }

        // ==========================================
        // PRIVATE UTILITY METHODS
        // ==========================================

        // Extracted reusable token evaluation logic
        private bool TryGetCurrentUserId(out int actorUserId)
        {
            var userIdClaim = User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out actorUserId);
        }
    }
}