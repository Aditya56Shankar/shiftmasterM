using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using shiftmaster.models;

namespace ShiftMaster.Application.Implementation
{
    public class RosterValidationService : IRosterValidationService
    {
        private readonly IShiftRepository _shiftRepo;
        private readonly ILeaveRepository _leaveRepo;
        private readonly ISkillRepository _skillRepo;
        private readonly IViolationRepository _violationRepo;
        private readonly IStatusCheckRepository _statusRepo;
        private readonly IAvailabilityRepository _availabilityRepo;

        public RosterValidationService(
            IShiftRepository shiftRepo,
            ILeaveRepository leaveRepo,
            ISkillRepository skillRepo,
            IViolationRepository violationRepo,
            IStatusCheckRepository statusRepo,
            IAvailabilityRepository availabilityRepo)
        {
            _shiftRepo = shiftRepo;
            _leaveRepo = leaveRepo;
            _skillRepo = skillRepo;
            _violationRepo = violationRepo;
            _statusRepo = statusRepo;
            _availabilityRepo = availabilityRepo;
        }

        public async Task ValidateAssignmentConstraintsAsync(int assignmentId)
        {
            // Track violation state locally in-memory
            bool hasBlockingViolation = false;

            var shift = await _shiftRepo.GetShiftWithDetailsAsync(assignmentId);
            if (shift == null) return;

            int rosterId = shift.RosterID;
            int userId = shift.UserID;
            DateTime targetDate = shift.AssignedDate.Date;


            var weeklyShifts = (await _shiftRepo.GetWeeklyShiftsAsync(rosterId, userId))
                .Where(s => s.Status != ShiftAssignmentStatus.Cancelled &&
                            s.AssignmentID != assignmentId) // ✅ IMPORTANT
                .ToList();


            var leaves = await _leaveRepo.GetActiveLeavesAsync(userId);

            var skills = await _skillRepo.GetEmployeeSkillsAsync(userId)
                         ?? new List<string>();

            

            // RULE 1: Leave
            if (HasLeaveConflict(leaves, targetDate))
            {
                hasBlockingViolation = true; // 👈 Set flag
                await AddViolation(rosterId, userId, ViolationType.UnavailableEmployee, SeverityLevel.Blocking);
            }

            

            // ✅ RULE 2: Combined Max Hours Rule

            // Step 1: calculate hours
            double existingHours = CalculateWeeklyHours(weeklyShifts);

            var duration = shift.EndTime - shift.StartTime;
            double newShiftHours = duration.TotalHours > 0
                ? duration.TotalHours
                : duration.Add(TimeSpan.FromDays(1)).TotalHours;

            double totalHours = existingHours + newShiftHours;

            // ✅ Fetch availability
            var availability = await _availabilityRepo
                .GetAvailabilityAsync(userId, targetDate);

            // ✅ Rule A: User MaxHours check
            double userMax = (double)(availability?.MaxHoursPerWeek ?? 40);

            if (userMax > 48)
            {

                hasBlockingViolation = true;

                await AddViolation(rosterId, userId,
                    ViolationType.MaxHoursExceeded,
                    SeverityLevel.Blocking);

                // ✅ FORCE CANCEL ALL SHIFTS
                var allShifts = await _shiftRepo.GetUserAssignmentsAsync(rosterId, userId);

                foreach (var s in allShifts)
                {
                    s.Status = ShiftAssignmentStatus.Cancelled;
                }

                await _shiftRepo.SaveAsync();

                return;

            }

            // ✅ Rule B: Fixed limits
            double maxHours = 40;
            double hardLimit = 48;

            if (totalHours > hardLimit)
            {
                hasBlockingViolation = true;

                await AddViolation(rosterId, userId,
                    ViolationType.MaxHoursExceeded,
                    SeverityLevel.Blocking);
            }
            else if (totalHours > maxHours)
            {
                await AddViolation(rosterId, userId,
                    ViolationType.MaxHoursExceeded,
                    SeverityLevel.Warning);
            }
            // RULE 3: Rest
            if (HasRestViolation(shift, weeklyShifts))
            {
                hasBlockingViolation = true; // 👈 Set flag
                await AddViolation(rosterId, userId, ViolationType.InsufficientRest, SeverityLevel.Blocking);
            }


            bool hasSkill = HasRequiredSkill(skills, shift);

            Console.WriteLine($"Has skill result: {hasSkill}");

            if (!hasSkill)
            {

                hasBlockingViolation = true; 

    await AddViolation(rosterId, userId,
        ViolationType.SkillGap,
        SeverityLevel.Blocking);

            }





            await _violationRepo.SaveAsync();

            // Check DB state
            bool hasBlockingFromDb = await _violationRepo
                .HasBlockingViolationAsync(rosterId, userId);

            bool hasBlocking = hasBlockingViolation || hasBlockingFromDb;

            if (hasBlocking)
            {
                var allShifts = await _shiftRepo.GetUserAssignmentsAsync(rosterId, userId);

                foreach (var s in allShifts)
                {
                    s.Status = ShiftAssignmentStatus.Cancelled;
                }

                await _shiftRepo.SaveAsync();
            }
            else
            {
                await UpdateShiftStatus(shift, userId, targetDate, false);

                await _shiftRepo.SaveAsync();
            }




        }
        // ==========================================
        // ✅ HELPER METHOD IMPLEMENTATIONS
        // ==========================================

        private async Task AddViolation(int rosterId, int userId, ViolationType type, SeverityLevel severity)
        {
            await _violationRepo.AddViolationAsync(new SchedulingConstraintViolation
            {
                RosterID = rosterId,
                UserID = userId,
                ViolationType = type,
                Severity = severity,
                Status = ViolationStatus.Open
            });
        }

        private bool HasLeaveConflict(List<LeaveBlock> leaves, DateTime targetDate)
        {
            return leaves.Any(lb =>
                targetDate >= lb.StartDate.Date &&
                targetDate <= lb.EndDate.Date);
        }

        private double CalculateWeeklyHours(List<ShiftAssignment> shifts)
        {
            double total = 0;
            foreach (var s in shifts)
            {
                var duration = s.EndTime - s.StartTime;
                total += duration.TotalHours > 0
                    ? duration.TotalHours
                    : duration.Add(TimeSpan.FromDays(1)).TotalHours;
            }
            return total;
        }

        private bool HasRestViolation(ShiftAssignment shift, List<ShiftAssignment> weeklyShifts)
        {
            var targetDate = shift.AssignedDate.Date;

            DateTime newStart = targetDate.Add(shift.StartTime);
            DateTime newEnd = targetDate.Add(shift.EndTime);

            // Handle overnight shifts
            if (newEnd <= newStart)
                newEnd = newEnd.AddDays(1);

            foreach (var other in weeklyShifts)
            {
                if (other.AssignmentID == shift.AssignmentID)
                    continue;

                if (other.Status == ShiftAssignmentStatus.Cancelled)
                    continue;

                DateTime otherStart = other.AssignedDate.Date.Add(other.StartTime);
                DateTime otherEnd = other.AssignedDate.Date.Add(other.EndTime);

                if (otherEnd <= otherStart)
                    otherEnd = otherEnd.AddDays(1);

                // ✅ CASE 1: Overlap (including same time)
                if (newStart < otherEnd && newEnd > otherStart)
                {
                    return true; // ❌ Violation
                }

                // ✅ CASE 2: Gap after other shift
                double gapAfter = (newStart - otherEnd).TotalHours;

                // ✅ CASE 3: Gap before other shift
                double gapBefore = (otherStart - newEnd).TotalHours;

                // ✅ 11-hour rule
                if ((gapAfter > 0 && gapAfter < 11) ||
                    (gapBefore > 0 && gapBefore < 11))
                {
                    return true; // ❌ Violation
                }
            }

            return false; // ✅ No violation
        }


        private bool HasRequiredSkill(List<string> skills, ShiftAssignment shift)
        {
            // ✅ If no skills → invalid

            if (string.IsNullOrWhiteSpace(shift.Role))
                return false;

            // ✅ STRICT: must have EXACT match
            return skills != null &&
                   skills.Count > 0 &&
                   skills.Any(skill =>
                       skill != null &&
                       skill.Trim().Equals(shift.Role.Trim(), StringComparison.OrdinalIgnoreCase));

        }

        private async Task UpdateShiftStatus(ShiftAssignment shift, int userId, DateTime targetDate, bool hasAnyViolation)
        {
            // 1. If there is a blocking violation, cancel it immediately and stop.
            if (hasAnyViolation)
            {
                shift.Status = ShiftAssignmentStatus.Cancelled;
                return;
            }

            // 2. If it's safe, figure out what specialized state it belongs to.
            if (await _statusRepo.IsCoveredAsync(userId))
            {
                shift.Status = ShiftAssignmentStatus.Covered;
            }
            else if (await _statusRepo.IsSwappedAsync(userId))
            {
                shift.Status = ShiftAssignmentStatus.Swapped;
            }
            else if (await _statusRepo.IsConfirmedAsync(userId, targetDate))
            {
                shift.Status = ShiftAssignmentStatus.Confirmed;
            }
            else
            {
                // 3. Default state if it's just a normal, valid shift.
                shift.Status = ShiftAssignmentStatus.Assigned;
            }
        }
    }
}
    