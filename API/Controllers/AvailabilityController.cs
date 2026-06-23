using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAvailabilityService service;

        public AvailabilityController(IMapper mapper, IAvailabilityService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        // ✅ POST: Add Availability
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Availability([FromBody] AvailabilityRequestDto avail)
        {
            var entity = mapper.Map<AvailabilitySubmission>(avail);

            var result = await service.AddAvailableAsync(entity);

            return Ok(mapper.Map<AvailabilityResponseDto>(result));
        }

        // ✅ PUT: Update Status
        [HttpPut("{id}")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> UpdateAvailabilityStatus(int id, [FromQuery] AvailabilityStatus status)
        {
            try
            {
                var updated = await service.UpdateAvailabilityStatusAsync(id, status);

                if (!updated)
                    return NotFound("Availability not found");

                return Ok(new
                {
                    message = "Availability status updated successfully",
                    AvailabilityID = id,
                    UpdatedStatus = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}