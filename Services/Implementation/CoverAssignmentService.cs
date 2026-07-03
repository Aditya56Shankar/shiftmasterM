using AutoMapper;
using Domain.Enums;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;
using Domain.Repositories;
using shiftmaster.models;

namespace Services.Implementation
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
			// Load the original shift assignment
			var originalAssignment = await _repository.GetShiftAssignmentWithRosterAsync(shiftAssignmentId);

			if (originalAssignment == null)
			{
				throw new ResourceNotFoundException($"Shift assignment with ID {shiftAssignmentId} not found.");
			}

			// Get the assigned date, location, and department info
			var assignedDate = originalAssignment.AssignedDate;
			var locationId = originalAssignment.Roster.LocationID;
			var departmentId = originalAssignment.Roster.DepartmentID;

			// Load required skills for this location and department
			var requiredSkillNames = await _repository.GetRequiredSkillNamesAsync(locationId, departmentId);

			//Making the skill to be in set to make them unique
			var requiredSkillSet = new HashSet<string>(requiredSkillNames);

			// Get all active users at the same location, excluding the original assignee
			var potentialCoverers = await _repository.GetActiveUsersByLocationExceptAsync(locationId, originalAssignment.UserID);

			var eligibleCoverers = new List<CoverEligibilityDto>();

			foreach (var user in potentialCoverers)
			{
				// Check if user has an approved leave block on the assigned date
				var hasLeaveBlock = await _repository.HasApprovedLeaveBlockOnDateAsync(user.UserID, assignedDate);

				if (hasLeaveBlock)
					continue;

				// Check if user has conflicting shift assignment on the same date
				var hasConflictingShift = await _repository.HasConflictingShiftOnDateAsync(
					user.UserID,
					assignedDate,
					originalAssignment.StartTime,
					originalAssignment.EndTime);

				if (hasConflictingShift)
					continue;

				// Get user's skills
				var userSkills = await _repository.GetActiveUserSkillNamesAsync(user.UserID);

				var userSkillSet = new HashSet<string>(userSkills);

				// Calculate matching skills
				var matchingSkills = userSkillSet.Intersect(requiredSkillSet).ToList();

				if (matchingSkills.Count >= 0)
				{
					eligibleCoverers.Add(new CoverEligibilityDto
					{
						UserID = user.UserID,
						EmployeeID = user.EmployeeID,
						Name = user.Name,
						MatchingSkills = matchingSkills
					});
				}
			}

			return eligibleCoverers;
		}

		public async Task<CoverAssignmentResponseDto> AssignCoverAsync(CreateCoverAssignmentDto dto)
		{
			var coverAssignment = _mapper.Map<CoverAssignment>(dto);
            var originalAssignment = await _repository.GetShiftAssignmentByIdAsync(dto.OriginalAssignmentID);

            if (originalAssignment == null)
            {
                throw new ResourceNotFoundException($"Original shift assignment with ID {dto.OriginalAssignmentID} not found.");
            }

            await _repository.AddCoverAssignmentAsync(coverAssignment);

			// Update the original shift assignment status to Covered

			originalAssignment.Status = ShiftAssignmentStatus.Covered;
			await _repository.SaveChangesAsync();

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
				throw new InvalidWorkflowStateException("Cover assignment is not in assigned state.");
			}

			if (coverAssignment.CoveringUserID != actorUserId)
			{
				throw new InvalidWorkflowStateException("Only the assigned covering employee can confirm this cover request.");
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
