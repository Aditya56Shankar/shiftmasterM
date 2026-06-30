using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using shiftmaster.models;
using ShiftMaster.models;

namespace Data.Repositories
{
	public class ShiftSwapRepository : IShiftSwapRepository
	{
		private readonly ApplicationDbContext _context;

		public ShiftSwapRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<ShiftAssignment?> GetShiftAssignmentWithRosterAsync(int shiftAssignmentId)
		{
			return await _context.ShiftAssignments
				.Include(sa => sa.Roster)
				.FirstOrDefaultAsync(sa => sa.AssignmentID == shiftAssignmentId);
		}

		public async Task<List<User>> GetActiveUsersByLocationAndDepartmentExceptAsync(int locationId, int departmentId, int excludedUserId)
		{
			return await _context.Users
				.Include(u => u.Role)
				.Where(u => u.LocationID == locationId
					&& u.DepartmentID == departmentId
					&& u.Status == UserStatus.Active
					&& u.UserID != excludedUserId
					&& u.Role.roleName == "FrontLine Employee")
				.ToListAsync();
		}

		public async Task<bool> HasApprovedLeaveBlockOnDateAsync(int userId, DateTime date)
		{
			return await _context.LeaveBlocks
				.Where(lb => lb.UserID == userId
					&& lb.Status == LeaveStatus.Active
					&& lb.ApprovedByID != null
					&& lb.StartDate <= date
					&& lb.EndDate >= date)
				.AnyAsync();
		}

		public async Task<ShiftAssignment?> GetUserAssignmentInWeekAsync(int userId, DateTime weekStartDate, DateTime weekEndDate)
		{
			return await _context.ShiftAssignments
				.FirstOrDefaultAsync(sa => sa.UserID == userId
					&& sa.AssignedDate >= weekStartDate
					&& sa.AssignedDate <= weekEndDate);
		}

		public async Task<SwapRequest> AddSwapRequestAsync(SwapRequest swapRequest)
		{
			_context.SwapRequests.Add(swapRequest);
			await _context.SaveChangesAsync();
			return swapRequest;
		}

		public async Task<SwapRequest?> GetSwapRequestByIdAsync(int swapId)
		{
			return await _context.SwapRequests
				.FirstOrDefaultAsync(sr => sr.SwapID == swapId);
		}

		public async Task<ShiftAssignment?> GetShiftAssignmentByIdAsync(int assignmentId)
		{
			return await _context.ShiftAssignments
				.FirstOrDefaultAsync(sa => sa.AssignmentID == assignmentId);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
