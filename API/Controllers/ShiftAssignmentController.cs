using Data.Context;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;
using Microsoft.EntityFrameworkCore;
using AutoMapper; 
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftAssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRosterValidationService _validationService;
        private readonly IMapper _mapper; // Added internal read-only private field tracking

        // Inject IMapper into your constructor alongside existing services
        public ShiftAssignmentController(
            ApplicationDbContext context,
            IRosterValidationService validationService,
            IMapper mapper)
        {
            _context = context;
            _validationService = validationService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AssignShift([FromBody] CreateAssignmentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var rosterExists = await _context.WeeklyRosters.AnyAsync(r => r.RosterID == dto.RosterID);
            if (!rosterExists) return NotFound($"Parent Weekly Roster with ID {dto.RosterID} does not exist.");

            // Use AutoMapper to convert your DTO into the raw Database Entity Model object
            var assignment = _mapper.Map<ShiftAssignment>(dto);


            _context.ShiftAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            // run compliance evaluations behind the scenes
            await _validationService.ValidateAssignmentConstraintsAsync(assignment.AssignmentID);

            return Ok(assignment);
        }
    }
}