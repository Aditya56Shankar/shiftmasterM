using ShiftMaster.models;
using shiftmaster.models;

namespace Domain.Repositories
{
	public interface ICoverAssignmentRepository
	{
		Task<ShiftAssignment?> GetShiftAssignmentWithRosterAsync(int shiftAssignmentId);
		Task<List<string>> GetRequiredSkillNamesAsync(int locationId, int departmentId);
		Task<List<User>> GetActiveUsersByLocationExceptAsync(int locationId, int excludedUserId);
		Task<bool> HasApprovedLeaveBlockOnDateAsync(int userId, DateTime date);
		Task<bool> HasConflictingShiftOnDateAsync(int userId, DateTime date, TimeSpan startTime, TimeSpan endTime);
		Task<List<string>> GetActiveUserSkillNamesAsync(int userId);
		Task<CoverAssignment> AddCoverAssignmentAsync(CoverAssignment coverAssignment);
		Task<CoverAssignment?> GetCoverAssignmentByIdAsync(int coverId);
		Task<ShiftAssignment?> GetShiftAssignmentByIdAsync(int assignmentId);
		Task SaveChangesAsync();
	}
}
