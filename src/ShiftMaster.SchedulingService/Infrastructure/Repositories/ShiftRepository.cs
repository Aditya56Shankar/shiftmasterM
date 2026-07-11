using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Domain.Enums;
using ShiftMaster.SchedulingService.Domain.Models;
using ShiftMaster.SchedulingService.Infrastructure.Data;

namespace ShiftMaster.SchedulingService.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly SchedulingDbContext _context;

        public ShiftRepository(SchedulingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShiftAssignment>> GetByUserIdAsync(int userId)
        {
            return await _context.ShiftAssignments
                .Where(s => s.UserID == userId)
                .OrderBy(s => s.AssignedDate)
                .ToListAsync();
        }

        public async Task<ShiftAssignment?> GetShiftWithDetailsAsync(int assignmentId)
        {
            return await _context.ShiftAssignments
                .Include(sa => sa.Roster)
                .Include(sa => sa.Pattern)
                .FirstOrDefaultAsync(sa => sa.AssignmentID == assignmentId);
        }

        public async Task<List<ShiftAssignment>> GetWeeklyShiftsAsync(int rosterId, int userId)
        {
            return await _context.ShiftAssignments
                .Where(sa =>
                    sa.RosterID == rosterId &&
                    sa.UserID == userId &&
                    sa.Status != ShiftAssignmentStatus.Cancelled)
                .ToListAsync();
        }

        public async Task<List<ShiftAssignment>> GetUserAssignmentsAsync(int rosterId, int userId)
        {
            return await _context.ShiftAssignments
                .Where(s => s.RosterID == rosterId &&
                            s.UserID == userId)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetAssignedCountAsync(int rosterId, DateTime date, int patternId)
        {
            return await _context.ShiftAssignments
                .Where(s =>
                    s.RosterID == rosterId &&
                    s.AssignedDate.Date == date &&
                    s.ShiftPatternID == patternId &&
                    s.Status != ShiftAssignmentStatus.Cancelled)
                .Select(s => s.UserID)
                .Distinct()
                .CountAsync();
        }

        public async Task AddAsync(ShiftAssignment assignment)
        {
            await _context.ShiftAssignments.AddAsync(assignment);
        }

        public async Task<bool> ShiftExistsAsync(
            int userId,
            DateTime assignedDate,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            return await _context.ShiftAssignments.AnyAsync(s =>
                s.UserID == userId &&
                s.AssignedDate.Date == assignedDate.Date &&
                s.StartTime == startTime &&
                s.EndTime == endTime &&
                s.Status != ShiftAssignmentStatus.Cancelled);
        }
    }
}
