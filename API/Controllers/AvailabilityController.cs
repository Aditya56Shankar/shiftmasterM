using AutoMapper;
using Data.Context;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Implementation;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityRepository repository;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;

        public AvailabilityController(IMapper mapper, IAvailabilityRepository repository, ApplicationDbContext db)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.db = db;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Availablity([FromBody] AvailabilityRequestDto avail)
        {
            var res = mapper.Map<AvailabilitySubmission>(avail);
            await repository.AddAvailableAsync(res);
            return Ok(mapper.Map<AvailabilityResponseDto>(res));
        }

        [HttpPut]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> UpdateAvailabilityStatus(int id, AvailabilityStatus status)
        {
            var availability = await db.AvailabilitySubmissions.FindAsync(id);

            if (availability == null)
                return NotFound("Availability not found");

            // ✅ Optional: prevent duplicate update
            if (availability.Status == status)
                return BadRequest("Status is already set to this value");

            // ✅ Update only status
            availability.Status = status;

            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Availability status updated successfully",
                AvailabilityID = availability.AvailabilityID,
                UpdatedStatus = availability.Status
            });
        }
    }
}
