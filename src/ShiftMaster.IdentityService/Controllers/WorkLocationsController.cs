using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Application.DTOs;
using ShiftMaster.IdentityService.Application.Interfaces;
using ShiftMaster.IdentityService.Infrastructure.Data;

namespace ShiftMaster.IdentityService.Controllers
{
    [ApiController]
    [Route("api/worklocations")]
    public class WorkLocationsController : ControllerBase
    {
        private readonly IWorkLocationService _locationService;
        private readonly IdentityDbContext _context;

        public WorkLocationsController(IWorkLocationService locationService, IdentityDbContext context)
        {
            _locationService = locationService;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound($"Work location with ID {id} not found.");
            }
            return Ok(location);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateWorkLocationDto newLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdLocation = await _locationService.CreateLocationAsync(newLocation);
            return CreatedAtAction(nameof(GetById), new { id = createdLocation.LocationID }, createdLocation);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkLocationDto dto)
        {
            var updated = await _locationService.UpdateLocationAsync(id, dto);
            if (updated == null) return NotFound($"Work location with ID {id} not found.");
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _locationService.DeleteLocationAsync(id);
                if (!success) return NotFound($"Work location with ID {id} not found.");
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ==========================================
        // INTERNAL HELPER ENDPOINTS (UNAUTHENTICATED)
        // ==========================================

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> LocationExists(int id)
        {
            var exists = await _context.WorkLocations.AnyAsync(w => w.LocationID == id);
            return Ok(exists);
        }

        [HttpGet("internal/{id}/name")]
        public async Task<IActionResult> GetLocationNameInternal(int id)
        {
            var loc = await _context.WorkLocations.FindAsync(id);
            if (loc == null) return NotFound();
            return Ok(loc.LocationName);
        }
    }
}
