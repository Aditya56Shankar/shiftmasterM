using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Data.Context;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;
using shiftmaster.models;
using shiftMaster.Services.DTOs;

namespace API.Controllers
{
    [Route("api/shiftassignment")]
    [ApiController]
    public class ShiftAssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRosterValidationService _validationService;
        private readonly IShiftRepository _shiftRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ShiftAssignmentController> _logger;

        public ShiftAssignmentController(IRosterValidationService validationService,IShiftRepository shiftRepo,ILogger<ShiftAssignmentController> logger,IMapper mapper)
        {
            _validationService = validationService;
            _shiftRepo = shiftRepo;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpPost]
        [Authorize(Roles = "Shift Supervisor")]
        public async Task<IActionResult> AssignShift(CreateAssignmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool duplicateExists =
                    await _shiftRepo.ShiftExistsAsync(
                        dto.UserID,
                        dto.AssignedDate,
                        dto.StartTime,
                        dto.EndTime);

                if (duplicateExists)
                {
                    return BadRequest(new
                    {
                        Message =
                            $"Employee {dto.UserID} already has a shift assigned on " +
                            $"{dto.AssignedDate:yyyy-MM-dd} from {dto.StartTime} to {dto.EndTime}."
                    });
                }

                var assignment = _mapper.Map<ShiftAssignment>(dto);

                await _shiftRepo.AddAsync(assignment);
                await _shiftRepo.SaveAsync();

                await _validationService
                    .ValidateAssignmentConstraintsAsync(
                        assignment.AssignmentID);



                var updatedAssignment =
                    await _shiftRepo.GetShiftWithDetailsAsync(
                        assignment.AssignmentID);

                var response =
                    _mapper.Map<AssignmentResponseDto>(
                        updatedAssignment);

                return Ok(response);


            }
            catch (InvalidWorkflowStateException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
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