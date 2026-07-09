using System.Security.Claims;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/leave")]
    public class LeaveBlocksController : ControllerBase
    {
        private readonly ILeaveBlockService service;
        private readonly IMapper mapper;

        public LeaveBlocksController(ILeaveBlockService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveBlockRequestDto leave)
        {
            if (leave == null)
                return BadRequest("Invalid request");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out var actorUserId))
                return Unauthorized("Invalid user.");

            try
            {
                var entity = mapper.Map<LeaveBlock>(leave);
                entity.UserID = actorUserId;

                var saved = await service.AddLeaveBlockAsync(entity);

                var response = mapper.Map<LeaveBlockResponseDto>(saved);

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
        public async Task<IActionResult> GetMyLeaves()
        {
            if (!TryGetCurrentUserId(out var userId))
                return Unauthorized("Invalid user.");

            try
            {
                var result = await service.GetLeavesForUserAsync(userId);
                return Ok(mapper.Map<List<LeaveBlockResponseDto>>(result));
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

        [HttpGet("pending")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> GetPendingLeaves([FromQuery] int locationId)
        {
            if (locationId <= 0)
                return BadRequest("Invalid locationId.");

            try
            {
                var result = await service.GetPendingLeavesByLocationAsync(locationId);
                return Ok(mapper.Map<List<LeaveBlockResponseDto>>(result));
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

        [HttpGet("{id}")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> GetLeaveById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid leave id.");
            }

            try
            {
                var result = await service.GetLeaveByIdAsync(id);
                if (result == null)
                {
                    return NotFound("Leave not found");
                }

                return Ok(mapper.Map<LeaveBlockResponseDto>(result));
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid leave id.");

            try
            {
                if (!TryGetCurrentUserId(out var approvedBy))
                    return Unauthorized("Invalid user.");

                await service.ApproveLeaveAsync(id, approvedBy);

                return Ok(new
                {
                    message = "Leave approved",
                    LeaveID = id,
                    UpdatedStatus = LeaveStatus.Active,
                    ApprovedBy = approvedBy
                });
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

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> CancelLeaveAsSupervisor(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid leave id.");

            try
            {
                if (!TryGetCurrentUserId(out var actingUserId))
                    return Unauthorized("Invalid user.");

                await service.CancelLeaveAsync(id, actingUserId, true);

                return Ok(new { message = "Leave cancelled", LeaveID = id, UpdatedStatus = LeaveStatus.Cancelled });
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> CancelLeaveAsEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid leave id.");

            try
            {
                if (!TryGetCurrentUserId(out var actingUserId))
                    return Unauthorized("Invalid user.");

                await service.CancelLeaveAsync(id, actingUserId, false);

                return Ok(new { message = "Leave cancelled", LeaveID = id, UpdatedStatus = LeaveStatus.Cancelled });
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

        private bool TryGetCurrentUserId(out int actorUserId)
        {
            var userIdClaim = User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out actorUserId);
        }
    }
}
