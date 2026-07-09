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
                throw new ResourceNotFoundException($"Shift assignment with ID {shiftAssignmentId} not found.");
            }

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

                // Changed: fetch all assignments in week (excluding same day)
                var targetAssignments = await _repository.GetUserAssignmentsInWeekAsync(user.UserID, rosterWeekStart, rosterWeekEnd, assignedDate);

                var assignmentOptions = targetAssignments
                    .Select(a => new SwapAssignmentOptionDto
                    {
                        AssignmentID = a.AssignmentID,
                        AssignedDate = a.AssignedDate.Date
                    })
                    .ToList();

                eligibleTargets.Add(new SwapEligibilityDto
                {
                    UserID = user.UserID,
                    EmployeeID = user.EmployeeID,
                    Name = user.Name,
                    AvailableAssignments = assignmentOptions
                });
            }

            return eligibleTargets;
        }

        public async Task<SwapRequestResponseDto> CreateSwapRequestAsync(CreateSwapRequestDto dto, int actorUserId)
        {
            var requesterAssignment = await _repository.GetShiftAssignmentByIdAsync(dto.OriginalAssignmentID);
            if (requesterAssignment == null)
                throw new ResourceNotFoundException($"Original assignment with ID {dto.OriginalAssignmentID} not found.");

            if (requesterAssignment.UserID != actorUserId)
                throw new InvalidWorkflowStateException("Requester does not own the original assignment.");

            if (!dto.ProposedAssignmentID.HasValue)
                throw new InvalidWorkflowStateException("ProposedAssignmentID is required for swap request.");

            var targetAssignment = await _repository.GetShiftAssignmentByIdAsync(dto.ProposedAssignmentID.Value);
            if (targetAssignment == null)
                throw new ResourceNotFoundException($"Proposed assignment with ID {dto.ProposedAssignmentID.Value} not found.");

            if (targetAssignment.UserID != dto.TargetUserID)
                throw new InvalidWorkflowStateException("Target user does not own the proposed assignment.");

            if (targetAssignment.AssignmentID == requesterAssignment.AssignmentID)
                throw new InvalidWorkflowStateException("Original and proposed assignments cannot be the same.");

            var swapRequest = _mapper.Map<SwapRequest>(dto);
            swapRequest.RequesterUserID = actorUserId;

            await _repository.AddSwapRequestAsync(swapRequest);
            return _mapper.Map<SwapRequestResponseDto>(swapRequest);
        }

        public async Task<SwapRequestResponseDto> RespondToSwapAsync(int swapId, bool accepted,int actorUserId)
        {
            var swapRequest = await _repository.GetSwapRequestByIdAsync(swapId);

            if (swapRequest == null)
            {
                throw new ResourceNotFoundException($"Swap request with ID {swapId} not found.");
            }

            if(swapRequest.TargetUserID != actorUserId)
            {
                throw new InvalidWorkflowStateException("You are not allowed to do this transaction");
            }

            if (swapRequest.ProposedAssignmentID.HasValue)
            {
                var shiftDetails = await _repository.GetShiftAssignmentByIdAsync(swapRequest.ProposedAssignmentID.Value);

                if(shiftDetails == null)
                {
                    throw new ResourceNotFoundException("Proposed assignment is not found");
                }

                if (shiftDetails.UserID != actorUserId)
                {
                    throw new InvalidWorkflowStateException("You are not authorized to accept the swap request.");
                }
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