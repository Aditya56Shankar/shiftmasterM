using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Data;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Clients;

namespace ShiftMaster.SchedulingService.Repositories
{
    public class ShiftSwapRepository : IShiftSwapRepository
    {
        private readonly SchedulingDbContext _context;
        private readonly IIdentityClient _identityClient;
        private readonly IEmployeeClient _employeeClient;

        public ShiftSwapRepository(SchedulingDbContext context, IIdentityClient identityClient, IEmployeeClient employeeClient)
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

        public async Task<List<UserShortDto>> GetActiveUsersByLocationAndDepartmentExceptAsync(int locationId, int departmentId, int excludedUserId)
        {
            var users = await _identityClient.GetActiveUsersByLocationAndDeptAsync(locationId, departmentId);
            return users.Where(u => u.UserID != excludedUserId).ToList();
        }

        public async Task<bool> HasApprovedLeaveBlockOnDateAsync(int userId, DateTime date)
        {
            return await _employeeClient.IsOnLeaveAsync(userId, date);
        }

        public async Task<List<ShiftAssignment>> GetUserAssignmentsInWeekAsync(int userId, DateTime weekStartDate, DateTime weekEndDate, DateTime assignedDate)
        {
            return await _context.ShiftAssignments
                .Where(sa => sa.UserID == userId
                    && sa.AssignedDate != assignedDate
                    && sa.AssignedDate >= weekStartDate
                    && sa.AssignedDate <= weekEndDate)
                .OrderBy(sa => sa.AssignedDate)
                .ThenBy(sa => sa.AssignmentID)
                .ToListAsync();
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
                .Include(sr => sr.OriginalAssignment)
                .Include(sr => sr.ProposedAssignment)
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
