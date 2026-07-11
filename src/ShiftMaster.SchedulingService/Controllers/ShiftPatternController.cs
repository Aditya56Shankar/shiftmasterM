using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.SchedulingService.DTOs;
using ShiftMaster.SchedulingService.Services;

namespace ShiftMaster.SchedulingService.Controllers
{
    [ApiController]
    [Route("api/shiftpattern")]
    public class ShiftPatternsController : ControllerBase
    {
        private readonly IShiftPatternService _service;

        public ShiftPatternsController(IShiftPatternService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Scheduling Admin")]
        public async Task<ActionResult<IEnumerable<ShiftPatternDto>>> GetAll()
        {
            var items = await _service.GetAllPatternsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Scheduling Admin")]
        public async Task<ActionResult<ShiftPatternDto>> GetById(int id)
        {
            var item = await _service.GetPatternByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Scheduling Admin")]
        public async Task<ActionResult<ShiftPatternDto>> Create([FromBody] CreateShiftPatternDto dto)
        {
            var created = await _service.CreatePatternAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.PatternID }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Scheduling Admin")]
        public async Task<ActionResult<ShiftPatternDto>> Update(int id, [FromBody] CreateShiftPatternDto dto)
        {
            var updated = await _service.UpdatePatternAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Scheduling Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var successful = await _service.DeletePatternAsync(id);
                if (!successful) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
