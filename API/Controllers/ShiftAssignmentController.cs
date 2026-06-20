using System.Threading.Tasks;
using AutoMapper; 
using Data.Context;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

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
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> AssignShift([FromBody] CreateAssignmentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var rosterExists = await _context.WeeklyRosters.AnyAsync(r => r.RosterID == dto.RosterID);
            if (!rosterExists) return NotFound();

            var assignment = _mapper.Map<ShiftAssignment>(dto);

            _context.ShiftAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            await _validationService.ValidateAssignmentConstraintsAsync(assignment.AssignmentID);

            // ✅ RELOAD FROM DATABASE
            var updatedAssignment = await _context.ShiftAssignments
                .AsNoTracking()    // ✅ VERY IMPORTANT
                .FirstOrDefaultAsync(a => a.AssignmentID == assignment.AssignmentID);

            return Ok(updatedAssignment);
        }
    }
}