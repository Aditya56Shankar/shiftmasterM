using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;

namespace API.Controllers
{
	[ApiController]
	[Route("api/covers")]
	public class CoverAssignmentController : ControllerBase
	{
		private readonly ICoverAssignmentService _service;

		public CoverAssignmentController(ICoverAssignmentService service)
		{
			_service = service;
		}

		[HttpGet("eligible")]
		[Authorize(Roles = "Shift Supervisor")]
		public async Task<IActionResult> GetEligibleCovers([FromQuery] int shiftAssignmentId)
		{
			try
			{
				var eligibleCovers = await _service.GetEligibleCoversAsync(shiftAssignmentId);
				return Ok(eligibleCovers);
			}
			catch (ResourceNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost("assign")]
		[Authorize(Roles = "Shift Supervisor")]
		public async Task<IActionResult> AssignCover([FromBody] CreateCoverAssignmentDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var coverAssignment = await _service.AssignCoverAsync(dto);
				return CreatedAtAction(nameof(AssignCover), coverAssignment);
			}
			catch (ResourceNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost("{coverId}/confirm")]
		[Authorize(Roles = "FrontLine Employee")]
		public async Task<IActionResult> ConfirmCover(int coverId, [FromQuery] int actorUserId)
		{
			if (coverId <= 0 || actorUserId <= 0)
    		{
        		return BadRequest("coverId and UserID must be greater than 0.");
    		}
			try
			{
				var coverAssignment = await _service.ConfirmCoverAsync(coverId, actorUserId);
				return Ok(coverAssignment);
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
	}
}
