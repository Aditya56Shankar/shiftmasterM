using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using shiftmaster.models;
using ShiftMaster.models;

namespace Data.Repositories
{
	public class CoverAssignmentRepository : ICoverAssignmentRepository
	{
		private readonly ApplicationDbContext _context;

		public CoverAssignmentRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<ShiftAssignment?> GetShiftAssignmentWithRosterAsync(int shiftAssignmentId)
		{
			return await _context.ShiftAssignments
				.Include(sa => sa.Roster)
				.FirstOrDefaultAsync(sa => sa.AssignmentID == shiftAssignmentId);
		}

		public async Task<List<string>> GetRequiredSkillNamesAsync(int locationId, int departmentId)
		{
			return await _context.SkillRequirements
				.Where(sr => sr.LocationID == locationId && sr.DepartmentID == departmentId && sr.Status == ActiveStatus.Active)
				.Select(sr => sr.SkillName)
				.ToListAsync();
		}

		public async Task<List<User>> GetActiveUsersByLocationExceptAsync(int locationId, int excludedUserId)
		{
			return await _context.Users
				.Where(u => u.LocationID == locationId && u.Status == UserStatus.Active && u.UserID != excludedUserId && u.Role.roleName == "FrontLine Employee")
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

		public async Task<bool> HasConflictingShiftOnDateAsync(int userId, DateTime date, TimeSpan startTime, TimeSpan endTime)
		{
			return await _context.ShiftAssignments
				.Where(sa => sa.UserID == userId
					&& sa.AssignedDate == date
					&& sa.StartTime < endTime
					&& sa.EndTime > startTime)
				.AnyAsync();
		}

		public async Task<List<string>> GetActiveUserSkillNamesAsync(int userId)
		{
			return await _context.EmployeeSkills
				.Where(es => es.UserID == userId && es.Status == ActiveStatus.Active)
				.Select(es => es.SkillName)
				.ToListAsync();
		}

		public async Task<CoverAssignment> AddCoverAssignmentAsync(CoverAssignment coverAssignment)
		{
			_context.CoverAssignments.Add(coverAssignment);
			await _context.SaveChangesAsync();
			return coverAssignment;
		}

		public async Task<CoverAssignment?> GetCoverAssignmentByIdAsync(int coverId)
		{
			return await _context.CoverAssignments
				.Include(c => c.OriginalAssignment)
				.FirstOrDefaultAsync(c => c.CoverID == coverId);
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
