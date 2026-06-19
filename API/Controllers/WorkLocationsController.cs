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
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            return Ok(locations);
        }

        // GET: api/worklocations/{id}
        [HttpGet("{id}")]
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
    }
}