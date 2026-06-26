using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftPatternsController : ControllerBase
    {
        private readonly IShiftPatternService _service;

        public ShiftPatternsController(IShiftPatternService service)
        {
            _service = service;
        }

        // 1. GET: api/shiftpatterns
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<IEnumerable<ShiftPatternDto>>> GetAll()
        {
            var items = await _service.GetAllPatternsAsync();
            return Ok(items);
        }

        // 2. GET: api/shiftpatterns/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ShiftPatternDto>> GetById(int id)
        {
            var item = await _service.GetPatternByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // 3. POST: api/shiftpatterns
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ShiftPatternDto>> Create([FromBody] CreateShiftPatternDto dto)
        {
            var created = await _service.CreatePatternAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.PatternID }, created);
        }

        // 4. PUT: api/shiftpatterns/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ShiftPatternDto>> Update(int id, [FromBody] CreateShiftPatternDto dto)
        {
            var updated = await _service.UpdatePatternAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // 5. DELETE: api/shiftpatterns/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

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