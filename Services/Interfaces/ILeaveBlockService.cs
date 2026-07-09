using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using shiftmaster.models;

namespace Services.Interfaces
{
    public interface ILeaveBlockService
    {
        Task<LeaveBlock> AddLeaveBlockAsync(LeaveBlock leave);
        Task<LeaveBlock?> GetLeaveByIdAsync(int id);
        Task<List<LeaveBlock>> GetLeavesForUserAsync(int userId);
        Task<List<LeaveBlock>> GetPendingLeavesByLocationAsync(int locationId);
        Task<bool> ApproveLeaveAsync(int id, int approvedBy);
        Task<bool> CancelLeaveAsync(int id, int actingUserId, bool isSupervisor);

    }
}
