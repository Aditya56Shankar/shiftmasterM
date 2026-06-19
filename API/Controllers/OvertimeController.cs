using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;

namespace API.Controllers
{
	[ApiController]
	[Route("api/overtime")]
	public class OvertimeController : ControllerBase
	{
		private readonly IOvertimeService _service;

		public OvertimeController(IOvertimeService service)
		{
			_service = service;
		}

		[HttpGet("pending")]
		public async Task<IActionResult> GetPendingOvertime([FromQuery] int locationId)
		{
			var pendingOvertime = await _service.GetPendingOvertimeAsync(locationId);
			return Ok(pendingOvertime);
		}

		[HttpPost("log")]
		public async Task<IActionResult> LogOvertime([FromBody] CreateOvertimeDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var overtimeAuthorisation = await _service.LogOvertimeAsync(dto);
			return CreatedAtAction(nameof(LogOvertime), overtimeAuthorisation);
		}

		[HttpPut("{otId}/authorize")]
		public async Task<IActionResult> AuthoriseOvertime(int otId, [FromBody] AuthoriseOvertimeDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var overtimeAuthorisation = await _service.AuthoriseOvertimeAsync(otId, dto.AuthorisedByID, dto.Approved);
				return Ok(overtimeAuthorisation);
			}
			catch (ResourceNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
