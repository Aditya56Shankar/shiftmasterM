using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using shiftmaster.models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftMaster.Application.Implementation

{

    public class RosterValidationService : IRosterValidationService
    {

        private readonly ApplicationDbContext _context;


        public RosterValidationService(ApplicationDbContext context)

        {

            _context = context;

        }


        public async Task ValidateAssignmentConstraintsAsync(int assignmentId)
        {
            // 1. Fetch shift
            var shift = await _context.ShiftAssignments
                .FirstOrDefaultAsync(sa => sa.AssignmentID == assignmentId);

            if (shift == null) return;

            int rosterId = shift.RosterID;
            int userId = shift.UserID;
            DateTime targetDate = shift.AssignedDate.Date;

            // ✅ Fetch all required data
            var weeklyShifts = await _context.ShiftAssignments
                .Where(sa => sa.RosterID == rosterId && sa.UserID == userId && sa.Status != ShiftAssignmentStatus.Cancelled)
                .ToListAsync();

            var activeLeaves = await _context.LeaveBlocks
                .Where(lb => lb.UserID == userId && lb.Status == LeaveStatus.Active)
                .ToListAsync();

            var employeeSkills = await _context.EmployeeSkills
                .Where(es => es.UserID == userId && es.Status == ActiveStatus.Active)
                .Select(es => es.SkillName)
                .ToListAsync();

            // ✅ FIXED: correct mapping (NOT AssignmentID)
            var requiredSkills = await _context.SkillRequirements
                .Where(sr => sr.LocationID == shift.AssignmentID && sr.Status == ActiveStatus.Active)
                .Select(sr => sr.SkillName)
                .ToListAsync();

            // ✅ Fetch employee availability (for max hours)
            var availability = await _context.AvailabilitySubmissions
                .FirstOrDefaultAsync(a => a.UserID == userId);

            // ✅ Clear old violations
            var oldViolations = await _context.SchedulingConstraintViolations
                .Where(v => v.RosterID == rosterId && v.UserID == userId)
                .ToListAsync();

            if (oldViolations.Any())
            {
                _context.SchedulingConstraintViolations.RemoveRange(oldViolations);
            }

            // ----------------------------------------------------------------------// RULE 1: Leave check// ----------------------------------------------------------------------
            bool hasLeave = activeLeaves.Any(lb =>
            targetDate >= lb.StartDate.Date && targetDate <= lb.EndDate.Date);

            if (hasLeave)
            {
                _context.SchedulingConstraintViolations.Add(new SchedulingConstraintViolation
                {
                    RosterID = rosterId,
                    UserID = userId,
                    ViolationType = ViolationType.UnavailableEmployee,
                    Severity = SeverityLevel.Blocking,
                    Status = ViolationStatus.Open
                });
            }

            // ----------------------------------------------------------------------// RULE 2: Weekly hours (✅ FIXED)// ----------------------------------------------------------------------
            double totalWeeklyHours = 0;

            foreach (var s in weeklyShifts)
            {
                var duration = s.EndTime - s.StartTime;

                totalWeeklyHours += duration.TotalHours > 0
                    ? duration.TotalHours
                    : duration.Add(TimeSpan.FromDays(1)).TotalHours;
            }

            decimal maxAllowedHours = availability?.MaxHoursPerWeek ?? 40m;

            if ((decimal)totalWeeklyHours > maxAllowedHours)
            {
                _context.SchedulingConstraintViolations.Add(new SchedulingConstraintViolation
                {
                    RosterID = rosterId,
                    UserID = userId,
                    ViolationType = ViolationType.MaxHoursExceeded,
                    Severity = SeverityLevel.Warning,
                    Status = ViolationStatus.Open
                });
            }

            // ----------------------------------------------------------------------// RULE 3: Rest gap// ----------------------------------------------------------------------
            foreach (var other in weeklyShifts.Where(s => s.AssignmentID != assignmentId))
            {
                if (Math.Abs((other.AssignedDate.Date - targetDate).TotalDays) <= 1)
                {
                    DateTime start1 = targetDate.Add(shift.StartTime);
                    DateTime end1 = targetDate.Add(shift.EndTime);

                    DateTime start2 = other.AssignedDate.Date.Add(other.StartTime);
                    DateTime end2 = other.AssignedDate.Date.Add(other.EndTime);

                    double gap1 = (start1 - end2).TotalHours;
                    double gap2 = (start2 - end1).TotalHours;

                    if ((start1 < end2 && end1 > start2) ||
                        (gap1 > 0 && gap1 < 11) ||
                        (gap2 > 0 && gap2 < 11))
                    {
                        _context.SchedulingConstraintViolations.Add(new SchedulingConstraintViolation
                        {
                            RosterID = rosterId,
                            UserID = userId,
                            ViolationType = ViolationType.InsufficientRest,
                            Severity = SeverityLevel.Blocking,
                            Status = ViolationStatus.Open
                        });
                        break;
                    }
                }
            }

            // ----------------------------------------------------------------------// RULE 4: Skill check// ----------------------------------------------------------------------
            if (requiredSkills.Any())
            {
                bool missingSkills = requiredSkills.Except(employeeSkills).Any();

                if (missingSkills)
                {
                    _context.SchedulingConstraintViolations.Add(new SchedulingConstraintViolation
                    {
                        RosterID = rosterId,
                        UserID = userId,
                        ViolationType = ViolationType.SkillGap,
                        Severity = SeverityLevel.Blocking,
                        Status = ViolationStatus.Open
                    });
                }
            }

            // ----------------------------------------------------------------------// STATUS LOGIC// ----------------------------------------------------------------------
            if (shift.Status == ShiftAssignmentStatus.Cancelled)
            {
                await _context.SaveChangesAsync();
                return;
            }

            bool isCovered = await _context.CoverAssignments
                .AnyAsync(c => c.CoveringUserID == userId && c.Status == CoverStatus.Completed);

            if (isCovered)
            {
                shift.Status = ShiftAssignmentStatus.Covered;
                await _context.SaveChangesAsync();
                return;
            }

            bool isSwapped = await _context.SwapRequests
                .AnyAsync(s =>
                    (s.RequesterUserID == userId || s.TargetUserID == userId) &&
                    s.Status == ApprovalStatus.Approved);

            if (isSwapped)
            {
                shift.Status = ShiftAssignmentStatus.Swapped;
                await _context.SaveChangesAsync();
                return;
            }

            bool isConfirmed = await _context.AvailabilitySubmissions
                .AnyAsync(a =>
                    a.UserID == userId &&
                    a.Status == AvailabilityStatus.Acknowledged &&
                    a.WeekStartDate <= targetDate &&
                    a.WeekStartDate.AddDays(6) >= targetDate);

            if (isConfirmed)
            {
                shift.Status = ShiftAssignmentStatus.Confirmed;
                await _context.SaveChangesAsync();
                return;
            }

            // ✅ FINAL VIOLATION CHECK
            bool hasBlocking = await _context.SchedulingConstraintViolations
                .AnyAsync(v =>
                    v.RosterID == rosterId &&
                    v.UserID == userId &&
                    v.Severity == SeverityLevel.Blocking &&
                    v.Status == ViolationStatus.Open);

            shift.Status = hasBlocking ? ShiftAssignmentStatus.Cancelled
                : ShiftAssignmentStatus.Assigned;

            await _context.SaveChangesAsync();
        }
    }

}

