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

            // BATCH FETCH 1: Pull ALL shifts for this user for the entire week in ONE network trip
            var weeklyShifts = await _context.ShiftAssignments
                .Where(sa => sa.RosterID == rosterId && sa.UserID == userId && sa.Status != ShiftAssignmentStatus.Cancelled)
                .ToListAsync();

            // BATCH FETCH 2: Pull ALL active leaves for this user in ONE query
            var activeLeaves = await _context.LeaveBlocks
                .Where(lb => lb.UserID == userId && lb.Status == LeaveStatus.Active)
                .ToListAsync();

            // 🔍 BATCH FETCH 3: Fetch all active skill IDs this employee possesses
            var employeeSkills = await _context.EmployeeSkills
    .Where(es => es.UserID == userId && es.Status == ActiveStatus.Active)
    .Select(es => es.SkillName) // 👈 Changed from EmpSkillID to SkillName to match fields
    .ToListAsync();

            // 🔍 BATCH FETCH 4: Fetch all required skill names for this shift's operating location
            var requiredSkills = await _context.SkillRequirements
                .Where(sr => sr.LocationID == shift.AssignmentID && sr.Status == ActiveStatus.Active) // 👈 Links through LocationID
                .Select(sr => sr.SkillName) // 👈 Grab SkillName to compare with employee skills
                .ToListAsync();

            // 2. Clear old violations for this employee on this roster
            var oldViolations = await _context.SchedulingConstraintViolations
                .Where(v => v.RosterID == rosterId && v.UserID == userId)
                .ToListAsync();

            if (oldViolations.Any())
            {
                _context.SchedulingConstraintViolations.RemoveRange(oldViolations);
                await _context.SaveChangesAsync();
            }

            // ----------------------------------------------------------------------
            // RULE 1: Leave Blocks Verification (Lightning fast in-memory execution)
            // ----------------------------------------------------------------------
            bool hasLeave = activeLeaves.Any(lb => targetDate >= lb.StartDate.Date && targetDate <= lb.EndDate.Date);

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

            // ----------------------------------------------------------------------
            // RULE 2: Max Weekly Hours Limit (In-memory calculation)
            // ----------------------------------------------------------------------
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
                    totalWeeklyHours += (duration.Add(TimeSpan.FromDays(1))).TotalHours;
                }
            }

            if (totalWeeklyHours > 40.0)
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

            // ----------------------------------------------------------------------
            // RULE 3: Rest Turnaround Window Gap (Completely in-memory loop)
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

            // ----------------------------------------------------------------------
            // RULE 4: Skill Coverage Verification (Correctly Placed Outside Loops)
            // ----------------------------------------------------------------------
            if (requiredSkills.Any())
            {
                // Find if there are any required items missing from employee's collected skills
                bool missingRequiredSkills = requiredSkills.Except(employeeSkills).Any();

                if (missingRequiredSkills)
                {
                    _context.SchedulingConstraintViolations.Add(new SchedulingConstraintViolation
                    {
                        RosterID = rosterId,
                        UserID = userId,
                        ViolationType = ViolationType.SkillGap, // Matches your actual Enum declaration
                        Severity = SeverityLevel.Blocking,
                        Status = ViolationStatus.Open
                    });
                }
            }




            if (shift.Status == ShiftAssignmentStatus.Cancelled)
            {
                await _context.SaveChangesAsync();
                return;
            }



            bool isCovered = await _context.CoverAssignments
                .AnyAsync(c =>
                    c.CoveringUserID == shift.UserID &&
                    c.Status == CoverStatus.Completed);

            if (isCovered)
            {
                shift.Status = ShiftAssignmentStatus.Covered;
                await _context.SaveChangesAsync();
                return;
            }


            bool isSwapped = await _context.SwapRequests
                .AnyAsync(s =>
                    (s.RequesterUserID == shift.UserID ||
                     s.TargetUserID == shift.UserID) &&
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

            // ✅ 5. Violations
            bool hasBlockingViolation = await _context.SchedulingConstraintViolations
                .AnyAsync(v =>
                    v.RosterID == rosterId &&
                    v.UserID == userId &&
                    v.Severity == SeverityLevel.Blocking &&
                    v.Status == ViolationStatus.Open);

            shift.Status = hasBlockingViolation
                ? ShiftAssignmentStatus.Cancelled
                : ShiftAssignmentStatus.Assigned;




            // Save all newly tracked system violations in one single push
            await _context.SaveChangesAsync();
        }
    }

}

