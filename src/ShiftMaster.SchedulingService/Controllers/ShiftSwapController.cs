using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.SchedulingService.DTOs;
using ShiftMaster.SchedulingService.Exceptions;
using ShiftMaster.SchedulingService.Services;

namespace ShiftMaster.SchedulingService.Controllers
{
    [ApiController]
    [Route("api/swaps")]
    public class ShiftSwapController : ControllerBase
    {
        private readonly IShiftSwapService _service;

        public ShiftSwapController(IShiftSwapService service)
        {
            _service = service;
        }

        [HttpGet("eligible-targets")]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> GetEligibleSwapTargets([FromQuery] int shiftAssignmentId)
        {
            try
            {
                var eligibleTargets = await _service.GetEligibleSwapTargetsAsync(shiftAssignmentId);
                return Ok(eligibleTargets);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("request")]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> CreateSwapRequest([FromBody] CreateSwapRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized("Invalid user.");
            }

            try
            {
                var swapRequest = await _service.CreateSwapRequestAsync(dto, actorUserId);
                return CreatedAtAction(nameof(CreateSwapRequest), swapRequest);
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

        [HttpPut("{swapId}/respond")]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> RespondToSwap(int swapId, [FromBody] RespondToSwapDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized("Invalid user.");
            }
            try
            {
                var swapRequest = await _service.RespondToSwapAsync(swapId, dto.Accepted, actorUserId);
                return Ok(swapRequest);
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

        [HttpPut("{swapId}/approve")]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> ApproveSwap(int swapId, [FromBody] ApproveSwapDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized("Invalid user.");
            }
            try
            {
                var swapRequest = await _service.ApproveSwapAsync(swapId, actorUserId, dto.Approved);
                return Ok(swapRequest);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound("The Swap Id could not be found");
            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest("The Swap Request could not able to change");
            }
        }
    }
}
