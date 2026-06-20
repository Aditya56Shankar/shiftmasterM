using System.Security.Claims;
using AutoMapper;
using Data.Context;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;
using ShiftMaster.models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveBlocksController : ControllerBase
    {
        private readonly ILeaveBlockRepository repository;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;
        public LeaveBlocksController(ILeaveBlockRepository repository, IMapper mapper, ApplicationDbContext db)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveBlockRequestDto leave)
        {

            if (leave == null)
            {
                return BadRequest("enter correctly");
            }
            var res = mapper.Map<LeaveBlock>(leave);
            var saved = await repository.AddLeaveBlockAsync(res);
            var response = mapper.Map<LeaveBlockResponseDto>(saved);

            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Supervisor")]
        
        public async Task<IActionResult> UpdateLeaveStatus(int id, LeaveStatus status)
        {
            var leave = await db.LeaveBlocks.FindAsync(id);

            if (leave == null)
                return NotFound("Leave not found");

            // ✅ FIX: Use "nameid" instead of ClaimTypes
            var userIdClaim = User.FindFirst("nameid")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            // ✅ Update status
            leave.Status = status;

            // ✅ Only set ApprovedBy when approving
            if (status == LeaveStatus.Active)
            {
                if (leave.ApprovedByID != null)
                    return BadRequest("Leave already approved");

                leave.ApprovedByID = int.Parse(userIdClaim);
            }

            await db.SaveChangesAsync();

            return Ok(new
            {
                message = $"Leave status updated to {leave.Status}",
                leave.LeaveBlockID,
                leave.Status,
                ApprovedBy = leave.ApprovedByID
            });
        }
    }
}
