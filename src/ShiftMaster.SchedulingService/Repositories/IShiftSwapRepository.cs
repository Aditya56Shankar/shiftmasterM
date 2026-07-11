using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Clients;

namespace ShiftMaster.SchedulingService.Repositories
{
    public interface IShiftSwapRepository
    {
        Task<ShiftAssignment?> GetShiftAssignmentWithRosterAsync(int shiftAssignmentId);
        Task<List<UserShortDto>> GetActiveUsersByLocationAndDepartmentExceptAsync(int locationId, int departmentId, int excludedUserId);
        Task<bool> HasApprovedLeaveBlockOnDateAsync(int userId, DateTime date);
        Task<List<ShiftAssignment>> GetUserAssignmentsInWeekAsync(int userId, DateTime weekStartDate, DateTime weekEndDate, DateTime assignedDate);
        Task<SwapRequest> AddSwapRequestAsync(SwapRequest swapRequest);
        Task<SwapRequest?> GetSwapRequestByIdAsync(int swapId);
        Task<ShiftAssignment?> GetShiftAssignmentByIdAsync(int assignmentId);
        Task SaveChangesAsync();
    }
}
