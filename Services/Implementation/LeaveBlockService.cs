using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using Services.Interfaces;
using Services.Implementation.Exceptions;
using Domain.Repositories;
using shiftmaster.models;

namespace Services.Implementation
{

    public class LeaveBlockService : ILeaveBlockService
    {
        private readonly ILeaveBlockRepository repository;

        public LeaveBlockService(ILeaveBlockRepository repository)
        {
            this.repository = repository;
        }

        public async Task<LeaveBlock> AddLeaveBlockAsync(LeaveBlock leave)
        {
            if (leave == null)
                throw new InvalidWorkflowStateException("Invalid leave request payload.");

            if (leave.UserID <= 0)
                throw new InvalidWorkflowStateException("Invalid user ID.");

            var userExists = await repository.UserExistsAsync(leave.UserID);
            if (!userExists)
                throw new ResourceNotFoundException($"User with ID {leave.UserID} not found.");

            if (leave.StartDate == default || leave.EndDate == default)
                throw new InvalidWorkflowStateException("Invalid leave date range.");

            if (leave.EndDate < leave.StartDate)
                throw new InvalidWorkflowStateException("End date cannot be earlier than start date.");

            leave.Status = LeaveStatus.Pending;
            leave.ApprovedByID = null;

            await repository.AddAsync(leave);
            await repository.SaveAsync();

            return leave;
        }

        public async Task<LeaveBlock?> GetLeaveByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidWorkflowStateException("Invalid leave ID.");

            return await repository.GetByIdAsync(id);
        }

        public async Task<List<LeaveBlock>> GetLeavesForUserAsync(int userId)
        {
            if (userId <= 0)
                throw new InvalidWorkflowStateException("Invalid user ID.");

            var userExists = await repository.UserExistsAsync(userId);
            if (!userExists)
                throw new ResourceNotFoundException($"User with ID {userId} not found.");

            return await repository.GetByUserIdAsync(userId);
        }

        public async Task<List<LeaveBlock>> GetPendingLeavesByLocationAsync(int locationId)
        {
            if (locationId <= 0)
                throw new InvalidWorkflowStateException("Invalid location ID.");

            var locationExists = await repository.LocationExistsAsync(locationId);
            if (!locationExists)
                throw new ResourceNotFoundException($"Location with ID {locationId} not found.");

            return await repository.GetPendingByLocationAsync(locationId);
        }

        public async Task<bool> ApproveLeaveAsync(int id, int approvedBy)
        {
            if (id <= 0)
                throw new InvalidWorkflowStateException("Invalid leave ID.");

            if (approvedBy <= 0)
                throw new InvalidWorkflowStateException("Invalid approver user ID.");

            var approverExists = await repository.UserExistsAsync(approvedBy);
            if (!approverExists)
                throw new ResourceNotFoundException($"User with ID {approvedBy} not found.");

            var leave = await repository.GetByIdAsync(id);

            if (leave == null)
                throw new ResourceNotFoundException($"Leave request with ID {id} not found.");

            if (leave.Status != LeaveStatus.Pending)
                throw new InvalidWorkflowStateException("Only pending leave requests can be approved.");

            leave.Status = LeaveStatus.Active;
            leave.ApprovedByID = approvedBy;

            await repository.SaveAsync();
            return true;
        }

        public async Task<bool> CancelLeaveAsync(int id, int actingUserId, bool isSupervisor)
        {
            if (id <= 0)
                throw new InvalidWorkflowStateException("Invalid leave ID.");

            if (actingUserId <= 0)
                throw new InvalidWorkflowStateException("Invalid user ID.");

            var actingUserExists = await repository.UserExistsAsync(actingUserId);
            if (!actingUserExists)
                throw new ResourceNotFoundException($"User with ID {actingUserId} not found.");

            var leave = await repository.GetByIdAsync(id);

            if (leave == null)
                throw new ResourceNotFoundException($"Leave request with ID {id} not found.");

            if (!isSupervisor && leave.UserID != actingUserId)
                throw new InvalidWorkflowStateException("You can only cancel your own leave request.");

            if (!isSupervisor && leave.Status != LeaveStatus.Pending)
                throw new InvalidWorkflowStateException("Employees can only cancel pending leave requests.");

            if (leave.Status == LeaveStatus.Cancelled)
                throw new InvalidWorkflowStateException("Leave is already cancelled.");

            leave.Status = LeaveStatus.Cancelled;

            await repository.SaveAsync();
            return true;
        }
    }

}
