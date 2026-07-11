using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShiftMaster.SchedulingService.Enums;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Repositories;
using ShiftMaster.SchedulingService.DTOs;
using ShiftMaster.SchedulingService.Exceptions;

namespace ShiftMaster.SchedulingService.Services
{
    public class CoverAssignmentService : ICoverAssignmentService
    {
        private readonly ICoverAssignmentRepository _repository;
        private readonly IMapper _mapper;

        public CoverAssignmentService(ICoverAssignmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CoverEligibilityDto>> GetEligibleCoversAsync(int shiftAssignmentId)
        {
            var originalAssignment = await _repository.GetShiftAssignmentWithRosterAsync(shiftAssignmentId);

            if (originalAssignment == null)
            {
                throw new ResourceNotFoundException($"Shift assignment with ID {shiftAssignmentId} not found.");
            }

            var assignedDate = originalAssignment.AssignedDate;
            var locationId = originalAssignment.Roster.LocationID;
            var departmentId = originalAssignment.Roster.DepartmentID;

            var requiredSkillNames = await _repository.GetRequiredSkillNamesAsync(locationId, departmentId);
            var requiredSkillSet = new HashSet<string>(requiredSkillNames, StringComparer.OrdinalIgnoreCase);

            var potentialCoverers = await _repository.GetActiveUsersByLocationExceptAsync(locationId, originalAssignment.UserID);

            var eligibleCoverers = new List<CoverEligibilityDto>();

            foreach (var user in potentialCoverers)
            {
                var hasLeaveBlock = await _repository.HasApprovedLeaveBlockOnDateAsync(user.UserID, assignedDate);

                if (hasLeaveBlock)
                    continue;

                var hasConflictingShift = await _repository.HasConflictingShiftOnDateAsync(
                    user.UserID,
                    assignedDate,
                    originalAssignment.StartTime,
                    originalAssignment.EndTime);

                if (hasConflictingShift)
                    continue;

                var userSkills = await _repository.GetActiveUserSkillNamesAsync(user.UserID);
                var userSkillSet = new HashSet<string>(userSkills, StringComparer.OrdinalIgnoreCase);

                var matchingSkills = userSkillSet.Intersect(requiredSkillSet).ToList();

                eligibleCoverers.Add(new CoverEligibilityDto
                {
                    UserID = user.UserID,
                    EmployeeID = user.EmployeeID,
                    Name = user.Name,
                    MatchingSkills = matchingSkills
                });
            }

            return eligibleCoverers;
        }

        public async Task<CoverAssignmentResponseDto> AssignCoverAsync(CreateCoverAssignmentDto dto)
        {
            var coverAssignment = _mapper.Map<CoverAssignment>(dto);
            coverAssignment.Status = CoverStatus.Assigned;

            await _repository.AddCoverAssignmentAsync(coverAssignment);
            return _mapper.Map<CoverAssignmentResponseDto>(coverAssignment);
        }

        public async Task<CoverAssignmentResponseDto> ConfirmCoverAsync(int coverId, int actorUserId)
        {
            var coverAssignment = await _repository.GetCoverAssignmentByIdAsync(coverId);

            if (coverAssignment == null)
            {
                throw new ResourceNotFoundException($"Cover assignment with ID {coverId} not found.");
            }

            if (coverAssignment.Status != CoverStatus.Assigned)
            {
                throw new InvalidWorkflowStateException("Cover assignment is not in Assigned status.");
            }

            if (coverAssignment.CoveringUserID != actorUserId)
            {
                throw new InvalidWorkflowStateException("You are not authorized to confirm this cover assignment.");
            }

            var shiftAssignment = await _repository.GetShiftAssignmentByIdAsync(coverAssignment.OriginalAssignmentID);

            if (shiftAssignment == null)
            {
                throw new ResourceNotFoundException($"Original shift assignment with ID {coverAssignment.OriginalAssignmentID} not found.");
            }

            coverAssignment.Status = CoverStatus.Confirmed;
            shiftAssignment.UserID = coverAssignment.CoveringUserID;

            await _repository.SaveChangesAsync();
            return _mapper.Map<CoverAssignmentResponseDto>(coverAssignment);
        }
    }
}
