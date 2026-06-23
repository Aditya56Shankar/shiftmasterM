using System.Security.Claims;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveBlocksController : ControllerBase
    {
        private readonly ILeaveBlockService service;
        private readonly IMapper mapper;

        public LeaveBlocksController(ILeaveBlockService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveBlockRequestDto leave)
        {
            if (leave == null)
                return BadRequest("Invalid request");

            var entity = mapper.Map<LeaveBlock>(leave);

            var saved = await service.AddLeaveBlockAsync(entity);

            var response = mapper.Map<LeaveBlockResponseDto>(saved);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> UpdateLeaveStatus(int id, [FromQuery] LeaveStatus status)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token");

                int approvedBy = int.Parse(userIdClaim);

                var result = await service.UpdateLeaveStatusAsync(id, status, approvedBy);

                if (!result)
                    return NotFound("Leave not found");

                return Ok(new
                {
                    message = $"Leave status updated to {status}",
                    LeaveID = id,
                    UpdatedStatus = status,
                    ApprovedBy = approvedBy
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}