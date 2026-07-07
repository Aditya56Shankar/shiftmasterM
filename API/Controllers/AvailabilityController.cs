using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [Route("api/availability")]
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

        [HttpPost]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> Availability([FromBody] AvailabilityRequestDto avail)
        {
            try
            {
                var entity = mapper.Map<AvailabilitySubmission>(avail);

                var result = await service.AddAvailableAsync(entity);

                return Ok(mapper.Map<AvailabilityResponseDto>(result));
            }

            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Shift Supervisor")]
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


        [HttpGet]
        [Authorize(Roles = "FrontLine Employee")]
        public async Task<IActionResult> GetMySchedule()
        {
            try
            {
                var userIdClaim = User.FindFirst("nameid")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized();

                int userId = int.Parse(userIdClaim);

                var result = await service.GetMyScheduleAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred.",
                    Error = ex.Message
                });
            }
        }

    }
}