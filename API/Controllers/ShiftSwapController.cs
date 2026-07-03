using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;

namespace API.Controllers
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
			var eligibleTargets = await _service.GetEligibleSwapTargetsAsync(shiftAssignmentId);
			return Ok(eligibleTargets);
		}

		[HttpPost("request")]
		[Authorize(Roles = "FrontLine Employee")]
		public async Task<IActionResult> CreateSwapRequest([FromBody] CreateSwapRequestDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var swapRequest = await _service.CreateSwapRequestAsync(dto);
			return CreatedAtAction(nameof(CreateSwapRequest), swapRequest);
		}

		[HttpPut("{swapId}/respond")]
		[Authorize(Roles = "FrontLine Employee")]
		public async Task<IActionResult> RespondToSwap(int swapId, [FromBody] RespondToSwapDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var swapRequest = await _service.RespondToSwapAsync(swapId, dto.Accepted);
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

			try
			{
				var swapRequest = await _service.ApproveSwapAsync(swapId, dto.ApprovedByID, dto.Approved);
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
