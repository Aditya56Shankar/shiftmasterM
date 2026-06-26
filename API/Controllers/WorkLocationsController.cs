using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;

namespace API.Controllers
{
    // This tells .NET that this class handles web API requests
    [ApiController]
    // This automatically sets the URL route to /api/worklocations
    [Route("api/[controller]")]
    public class WorkLocationsController : ControllerBase
    {
        private readonly IWorkLocationService _locationService;

        public WorkLocationsController(IWorkLocationService locationService)
        {
            _locationService = locationService;
        }

        // GET: api/worklocations
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            return Ok(locations);
        }

        // GET: api/worklocations/{id}
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

        // POST: api/worklocations
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([FromBody] CreateWorkLocationDto newLocation)
        {
            // This checks if the incoming JSON matches your CreateWorkLocationDto structure
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdLocation = await _locationService.CreateLocationAsync(newLocation);

            // Returns an HTTP 201 Created status and points to the newly created resource
            return CreatedAtAction(nameof(GetById), new { id = createdLocation.LocationID }, createdLocation);
        }

        // PUT: api/worklocations/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkLocationDto dto)
        {
            var updated = await _locationService.UpdateLocationAsync(id, dto);
            if (updated == null) return NotFound($"Work location with ID {id} not found.");
            return Ok(updated);
        }

        // DELETE: api/worklocations/{id}
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
    }
}