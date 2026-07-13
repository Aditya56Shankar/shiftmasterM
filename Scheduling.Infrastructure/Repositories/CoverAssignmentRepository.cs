using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Clients;
using ShiftMaster.SchedulingService.Domain.Enums;
using ShiftMaster.SchedulingService.Domain.Models;
using ShiftMaster.SchedulingService.Infrastructure.Data;

namespace ShiftMaster.SchedulingService.Infrastructure.Repositories
{
    public class CoverAssignmentRepository : ICoverAssignmentRepository
    {
        private readonly SchedulingDbContext _context;
        private readonly IIdentityClient _identityClient;
        private readonly IEmployeeClient _employeeClient;

        public CoverAssignmentRepository(SchedulingDbContext context, IIdentityClient identityClient, IEmployeeClient employeeClient)
        {
            _context = context;
            _identityClient = identityClient;
            _employeeClient = employeeClient;
        }

        public async Task<ShiftAssignment?> GetShiftAssignmentWithRosterAsync(int shiftAssignmentId)
        {
            return await _context.ShiftAssignments
                .Include(sa => sa.Roster)
                .FirstOrDefaultAsync(sa => sa.AssignmentID == shiftAssignmentId);
        }

        public async Task<List<string>> GetRequiredSkillNamesAsync(int locationId, int departmentId)
        {
            return await _employeeClient.GetRequiredSkillNamesAsync(locationId, departmentId);
        }

        public async Task<List<UserShortDto>> GetActiveUsersByLocationExceptAsync(int locationId, int excludedUserId)
        {
            var users = await _identityClient.GetActiveUsersByLocationAsync(locationId);
            return users.Where(u => u.UserID != excludedUserId).ToList();
        }

        public async Task<bool> HasApprovedLeaveBlockOnDateAsync(int userId, DateTime date)
        {
            return await _employeeClient.IsOnLeaveAsync(userId, date);
        }

        public async Task<bool> HasConflictingShiftOnDateAsync(int userId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return await _context.ShiftAssignments
                .Where(sa => sa.UserID == userId
                    && sa.AssignedDate == date
                    && sa.StartTime < endTime
                    && sa.EndTime > startTime
                    && sa.Status != ShiftAssignmentStatus.Cancelled)
                .AnyAsync();
        }

        public async Task<List<string>> GetActiveUserSkillNamesAsync(int userId)
        {
            return await _employeeClient.GetEmployeeSkillsAsync(userId);
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
