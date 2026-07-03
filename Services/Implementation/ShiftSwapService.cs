using AutoMapper;
using Domain.Enums;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;
using Domain.Repositories;
using shiftmaster.models;

namespace Services.Implementation
{
	public class ShiftSwapService : IShiftSwapService
	{
		private readonly IShiftSwapRepository _repository;
		private readonly IMapper _mapper;

		public ShiftSwapService(IShiftSwapRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<List<SwapEligibilityDto>> GetEligibleSwapTargetsAsync(int shiftAssignmentId)
		{
			// Load the requesting shift assignment
			var requesterAssignment = await _repository.GetShiftAssignmentWithRosterAsync(shiftAssignmentId);

			if (requesterAssignment == null)
			{
				return new List<SwapEligibilityDto>();
			}

			var requesterId = requesterAssignment.UserID;
			var assignedDate = requesterAssignment.AssignedDate;
			var locationId = requesterAssignment.Roster.LocationID;
			var rosterWeekStart = requesterAssignment.Roster.WeekStartDate;
			var rosterWeekEnd = requesterAssignment.Roster.WeekEndDate;

			// Get all active users at the same location and department, excluding the requester
			var departmentId = requesterAssignment.Roster.DepartmentID;
			var potentialTargets = await _repository.GetActiveUsersByLocationAndDepartmentExceptAsync(locationId, departmentId, requesterId);

			var eligibleTargets = new List<SwapEligibilityDto>();

			foreach (var user in potentialTargets)
			{
				// Check if user has an approved leave block on the requester's assigned date
				var hasLeaveBlock = await _repository.HasApprovedLeaveBlockOnDateAsync(user.UserID, assignedDate);

				if (hasLeaveBlock)
					continue;

				// Find any shift assignment this user has in the same roster week
				var targetAssignment = await _repository.GetUserAssignmentInWeekAsync(user.UserID, rosterWeekStart, rosterWeekEnd, assignedDate);

				eligibleTargets.Add(new SwapEligibilityDto
				{
					UserID = user.UserID,
					EmployeeID = user.EmployeeID,
					Name = user.Name,
					AvailableAssignmentID = targetAssignment?.AssignmentID
				});
			}

			return eligibleTargets;
		}

		public async Task<SwapRequestResponseDto> CreateSwapRequestAsync(CreateSwapRequestDto dto)
		{
			var swapRequest = _mapper.Map<SwapRequest>(dto);

			await _repository.AddSwapRequestAsync(swapRequest);

			return _mapper.Map<SwapRequestResponseDto>(swapRequest);
		}

		public async Task<SwapRequestResponseDto> RespondToSwapAsync(int swapId, bool accepted)
		{
			var swapRequest = await _repository.GetSwapRequestByIdAsync(swapId);

			if (swapRequest == null)
			{
				throw new ResourceNotFoundException($"Swap request with ID {swapId} not found.");
			}

			if (swapRequest.Status != ApprovalStatus.Pending)
			{
				throw new InvalidWorkflowStateException("Swap request is not in pending state.");
			}

			swapRequest.Status = accepted ? ApprovalStatus.Approved : ApprovalStatus.Rejected;
			await _repository.SaveChangesAsync();

			return _mapper.Map<SwapRequestResponseDto>(swapRequest);
		}

		public async Task<SwapRequestResponseDto> ApproveSwapAsync(int swapId, int approvedById, bool approved)
		{
			var swapRequest = await _repository.GetSwapRequestByIdAsync(swapId);

			if (swapRequest == null)
			{
				throw new ResourceNotFoundException($"Swap request with ID {swapId} not found.");
			}

			if (swapRequest.Status != ApprovalStatus.Approved)
			{
				throw new InvalidWorkflowStateException("Swap request is not in approved (peer-accepted) state.");
			}

			if (approved)
			{
				// Load both assignments
				var originalAssignment = await _repository.GetShiftAssignmentByIdAsync(swapRequest.OriginalAssignmentID);

				var proposedAssignment = swapRequest.ProposedAssignmentID.HasValue
					? await _repository.GetShiftAssignmentByIdAsync(swapRequest.ProposedAssignmentID.Value)
					: null;

				if (originalAssignment == null)
				{
					throw new ResourceNotFoundException($"Original assignment with ID {swapRequest.OriginalAssignmentID} not found.");
				}

				if (proposedAssignment == null)
				{
					throw new ResourceNotFoundException("Proposed assignment was not found for this swap request.");
				}

				// Swap the UserIDs
				(originalAssignment.UserID, proposedAssignment.UserID) = (proposedAssignment.UserID, originalAssignment.UserID);

				// Set both to Swapped status
				originalAssignment.Status = ShiftAssignmentStatus.Swapped;
				proposedAssignment.Status = ShiftAssignmentStatus.Swapped;

				swapRequest.Status = ApprovalStatus.Completed;
				swapRequest.ApprovedByID = approvedById;
			}
			else
			{
				swapRequest.Status = ApprovalStatus.Rejected;
			}

			await _repository.SaveChangesAsync();

			return _mapper.Map<SwapRequestResponseDto>(swapRequest);
		}
	}
}
