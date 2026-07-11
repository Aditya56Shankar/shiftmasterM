using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Models;

namespace ShiftMaster.Employee.Services
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
