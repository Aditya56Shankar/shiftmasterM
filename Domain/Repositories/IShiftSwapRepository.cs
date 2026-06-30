using shiftmaster.models;
using ShiftMaster.models;

namespace Domain.Repositories
{
	public interface IShiftSwapRepository
	{
		Task<ShiftAssignment?> GetShiftAssignmentWithRosterAsync(int shiftAssignmentId);
		Task<List<User>> GetActiveUsersByLocationAndDepartmentExceptAsync(int locationId, int departmentId, int excludedUserId);
		Task<bool> HasApprovedLeaveBlockOnDateAsync(int userId, DateTime date);
		Task<ShiftAssignment?> GetUserAssignmentInWeekAsync(int userId, DateTime weekStartDate, DateTime weekEndDate);
		Task<SwapRequest> AddSwapRequestAsync(SwapRequest swapRequest);
		Task<SwapRequest?> GetSwapRequestByIdAsync(int swapId);
		Task<ShiftAssignment?> GetShiftAssignmentByIdAsync(int assignmentId);
		Task SaveChangesAsync();
	}
}
