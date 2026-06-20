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
	}
}
