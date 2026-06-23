using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Repositories;
using shiftmaster.models;

namespace Services.Implementation.Repositories
{

    public class ShiftRepository : IShiftRepository
    {
        private readonly ApplicationDbContext _context;

        public ShiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShiftAssignment> GetShiftWithDetailsAsync(int assignmentId)
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
    }

}
