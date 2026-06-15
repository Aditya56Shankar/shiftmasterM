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
            // 1. Fetch the target shift assignment record
            var shift = await _context.ShiftAssignments
                .FirstOrDefaultAsync(sa => sa.AssignmentID == assignmentId);

            if (shift == null || !shift.RosterID.HasValue) return;

            int rosterId = shift.RosterID.Value;
            int userId = shift.UserID;
            DateTime targetDate = shift.AssignedDate.Date;

            // 2. Clear old violations for this employee on this roster to re-evaluate cleanly
            var oldViolations = await _context.SchedulingConstraintViolations
                .Where(v => v.RosterID == rosterId && v.UserID == userId)
                .ToListAsync();

            if (oldViolations.Any())
            {
                _context.SchedulingConstraintViolations.RemoveRange(oldViolations);
                await _context.SaveChangesAsync();
            }

            // ----------------------------------------------------------------------
            // RULE 1: Leave Blocks Verification (UnavailableEmployee)
            // ----------------------------------------------------------------------
            bool hasLeave = await _context.LeaveBlocks
                .AnyAsync(lb => lb.UserID == userId &&
                                lb.Status == LeaveStatus.Active &&
                                targetDate >= lb.StartDate.Date &&
                                targetDate <= lb.EndDate.Date);

            if (hasLeave)
            {
                var leaveViolation = new SchedulingConstraintViolation
                {
                    RosterID = rosterId,
                    UserID = userId,
                    ViolationType = ViolationType.UnavailableEmployee,
                    Severity = SeverityLevel.Blocking,
                    Status = ViolationStatus.Open
                };
                _context.SchedulingConstraintViolations.Add(leaveViolation);
            }

            // ----------------------------------------------------------------------
            // RULE 2: Max Weekly Hours Limit (MaxHoursExceeded)
            // ----------------------------------------------------------------------
            var weeklyShifts = await _context.ShiftAssignments
                .Where(sa => sa.RosterID == rosterId && sa.UserID == userId && sa.Status != ShiftAssignmentStatus.Cancelled)
                .ToListAsync();

            double totalWeeklyHours = 0;
            foreach (var s in weeklyShifts)
            {
                var duration = s.EndTime - s.StartTime;
                if (duration.TotalHours > 0)
                {
                    totalWeeklyHours += duration.TotalHours;
                }
                else
                {
                    // Safe evaluation wrapper for midnight shift crossings
                    totalWeeklyHours += (duration.Add(TimeSpan.FromDays(1))).TotalHours;
                }
            }

            if (totalWeeklyHours > 40.0)
            {
                var hoursViolation = new SchedulingConstraintViolation
                {
                    RosterID = rosterId,
                    UserID = userId,
                    ViolationType = ViolationType.MaxHoursExceeded,
                    Severity = SeverityLevel.Warning,
                    Status = ViolationStatus.Open
                };
                _context.SchedulingConstraintViolations.Add(hoursViolation);
            }

            // ----------------------------------------------------------------------
            // RULE 3: Rest Turnaround Window Gap (InsufficientRest)
            // ----------------------------------------------------------------------
            foreach (var otherShift in weeklyShifts.Where(sa => sa.AssignmentID != assignmentId))
            {
                if (Math.Abs((otherShift.AssignedDate.Date - targetDate).TotalDays) <= 1)
                {
                    DateTime contextStartThis = targetDate.Add(shift.StartTime);
                    DateTime contextEndThis = targetDate.Add(shift.EndTime);
                    DateTime contextStartOther = otherShift.AssignedDate.Date.Add(otherShift.StartTime);
                    DateTime contextEndOther = otherShift.AssignedDate.Date.Add(otherShift.EndTime);

                    double gapAfterOther = (contextStartThis - contextEndOther).TotalHours;
                    double gapBeforeOther = (contextStartOther - contextEndThis).TotalHours;

                    if ((contextStartThis < contextEndOther && contextEndThis > contextStartOther) ||
                        (gapAfterOther > 0 && gapAfterOther < 11.0) ||
                        (gapBeforeOther > 0 && gapBeforeOther < 11.0))
                    {
                        var restViolation = new SchedulingConstraintViolation
                        {
                            RosterID = rosterId,
                            UserID = userId,
                            ViolationType = ViolationType.InsufficientRest,
                            Severity = SeverityLevel.Blocking,
                            Status = ViolationStatus.Open
                        };
                        _context.SchedulingConstraintViolations.Add(restViolation);
                        break;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}