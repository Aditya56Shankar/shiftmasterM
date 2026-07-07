using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Domain.Repositories;
using shiftmaster.models;
using Microsoft.Extensions.Logging;
using Services.Implementation.Exceptions;

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

        private readonly ILogger<RosterValidationService> _logger;




        public RosterValidationService(
            IShiftRepository shiftRepo,
            ILeaveRepository leaveRepo,
            ISkillRepository skillRepo,
            IViolationRepository violationRepo,
            IStatusCheckRepository statusRepo,
            IAvailabilityRepository availabilityRepo
,
            ILogger<RosterValidationService> logger)
        {
            _shiftRepo = shiftRepo;
            _leaveRepo = leaveRepo;
            _skillRepo = skillRepo;
            _violationRepo = violationRepo;
            _statusRepo = statusRepo;
            _availabilityRepo = availabilityRepo;
            _logger = logger;
        }

        public async Task ValidateAssignmentConstraintsAsync(int assignmentId)
        {
            

            bool hasBlockingViolation = false;

            var shift = await _shiftRepo.GetShiftWithDetailsAsync(assignmentId);

            if (shift == null)
            {
                

                throw new ResourceNotFoundException(
                    $"Shift assignment with ID {assignmentId} was not found.");
            }

            int rosterId = shift.RosterID;
            int userId = shift.UserID;
            DateTime targetDate = shift.AssignedDate.Date;

            

            var weeklyShifts = (await _shiftRepo.GetWeeklyShiftsAsync(rosterId, userId))
                .Where(s => s.Status != ShiftAssignmentStatus.Cancelled &&
                            s.AssignmentID != assignmentId)
                .ToList();

            bool duplicateShift = weeklyShifts.Any(s =>
                s.AssignedDate.Date == shift.AssignedDate.Date &&
                s.StartTime == shift.StartTime &&
                s.EndTime == shift.EndTime);

            if (duplicateShift)
            {
                

                shift.Status = ShiftAssignmentStatus.Cancelled;

                await _shiftRepo.SaveAsync();

                throw new InvalidWorkflowStateException(
                    $"Employee {userId} already has a shift assigned on {shift.AssignedDate:yyyy-MM-dd} from {shift.StartTime} to {shift.EndTime}.");
            }

            var leaves = await _leaveRepo.GetActiveLeavesAsync(userId);

            if (HasLeaveConflict(leaves, targetDate))
            {
                hasBlockingViolation = true;

                await AddViolation(
                    rosterId,
                    userId,
                    ViolationType.UnavailableEmployee,
                    SeverityLevel.Blocking);

                shift.Status = ShiftAssignmentStatus.Cancelled;

                await _violationRepo.SaveAsync();
                await _shiftRepo.SaveAsync();

                throw new InvalidWorkflowStateException(
                    $"Employee {userId} is on approved leave on {targetDate:yyyy-MM-dd}.");
            }



            var availability = await _availabilityRepo
                .GetAvailabilityAsync(userId, targetDate);

           

            if (availability == null)
            {

                await AddViolation(
                        rosterId,
                        userId,
                        ViolationType.UnavailableEmployee,
                        SeverityLevel.Blocking);

                shift.Status = ShiftAssignmentStatus.Cancelled;

                await _violationRepo.SaveAsync();
                await _shiftRepo.SaveAsync();

                throw new ResourceNotFoundException(
                    $"No availability submitted for employee {userId}.");

            }

            var assignedDay = targetDate.DayOfWeek.ToString();

            

            if (!availability.AvailableDays.Contains(assignedDay))
            {
                

                await AddViolation(
                    rosterId,
                    userId,
                    ViolationType.UnavailableEmployee,
                    SeverityLevel.Blocking);

                shift.Status = ShiftAssignmentStatus.Cancelled;

                await _violationRepo.SaveAsync();
                await _shiftRepo.SaveAsync();

                throw new InvalidWorkflowStateException(
                    $"Employee {userId} is not available on {assignedDay}.");
            }

            double existingHours = CalculateWeeklyHours(weeklyShifts);

            var duration = shift.EndTime - shift.StartTime;

            double newShiftHours =
                duration.TotalHours > 0
                ? duration.TotalHours
                : duration.Add(TimeSpan.FromDays(1)).TotalHours;

            double totalHours = existingHours + newShiftHours;



            double userMax = (double)availability.MaxHoursPerWeek;

            // Blocking violation (> 48 hours)
            if (totalHours > 48)
            {
                hasBlockingViolation = true;

                await AddViolation(
                    rosterId,
                    userId,
                    ViolationType.MaxHoursExceeded,
                    SeverityLevel.Blocking);

                var allShifts = await _shiftRepo
                    .GetUserAssignmentsAsync(rosterId, userId);

         
                    shift.Status = ShiftAssignmentStatus.Cancelled;
                

                await _shiftRepo.SaveAsync();

                throw new InvalidWorkflowStateException(
                    $"Employee weekly hours ({totalHours}) exceed the hard limit of 48 hours.");
            }

            // Warning violation (> 40 hours)
            if (totalHours > 40)
            {
                await AddViolation(
                    rosterId,
                    userId,
                    ViolationType.MaxHoursExceeded,
                    SeverityLevel.Warning);

                await _violationRepo.SaveAsync();

                _logger.LogWarning(
                    "Employee exceeded recommended weekly hours. UserId={UserId}, Hours={Hours}",
                    userId,
                    totalHours);
            }


            bool hasSkill = HasRequiredSkill(
                await _skillRepo.GetEmployeeSkillsAsync(userId)
                ?? new List<string>(),
                shift);



            if (!hasSkill)
            {
                hasBlockingViolation = true;

                await AddViolation(
                    rosterId,
                    userId,
                    ViolationType.SkillGap,
                    SeverityLevel.Blocking);

                shift.Status = ShiftAssignmentStatus.Cancelled;

                await _violationRepo.SaveAsync();
                await _shiftRepo.SaveAsync();

                throw new ResourceNotFoundException(
                    $"Employee {userId} does not possess the required skill '{shift.Role}'.");
            }



            if (HasRestViolation(shift, weeklyShifts))
            {
                await AddViolation(
                    rosterId,
                    userId,
                    ViolationType.InsufficientRest,
                    SeverityLevel.Blocking);

                shift.Status = ShiftAssignmentStatus.Cancelled;

                await _violationRepo.SaveAsync();
                await _shiftRepo.SaveAsync();

                throw new InvalidWorkflowStateException(
                    $"Insufficient rest period detected for employee {userId}. Minimum 11 hours rest is required between shifts.");
            }

            await _violationRepo.SaveAsync();

            /* bool hasBlockingFromDb =
                 await _violationRepo.HasBlockingViolationAsync(
                     rosterId,
                     userId);

             bool hasBlocking =
                 hasBlockingViolation || hasBlockingFromDb;

             if (hasBlocking)
             {


                 var allShifts =
                     await _shiftRepo.GetUserAssignmentsAsync(
                         rosterId,
                         userId);

                 foreach (var s in allShifts)
                 {
                     s.Status = ShiftAssignmentStatus.Cancelled;
                 }

                 await _shiftRepo.SaveAsync();
             }*/


            shift.Status = ShiftAssignmentStatus.Cancelled;

            await _shiftRepo.SaveAsync();


            if (shift.AssignedDate.Date < DateTime.Today)
            {
                

                await AddViolation(
                    rosterId,
                    userId,
                    ViolationType.InvalidRosterDate,
                    SeverityLevel.Blocking);

                shift.Status = ShiftAssignmentStatus.Cancelled;

                await _shiftRepo.SaveAsync();

                throw new InvalidWorkflowStateException(
                    $"Shift cannot be assigned for a past date ({shift.AssignedDate:yyyy-MM-dd}).");
            }

            await UpdateShiftStatus(
                shift,
                userId,
                targetDate,
                false);

            await _shiftRepo.SaveAsync();

            _logger.LogInformation(
                "Validation completed successfully. AssignmentId={AssignmentId}",
                assignmentId);
        }




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

        private bool HasRestViolation( ShiftAssignment currentShift, List<ShiftAssignment> existingShifts)
        {
            DateTime currentStart =
                currentShift.AssignedDate.Date + currentShift.StartTime;

            DateTime currentEnd =
                currentShift.AssignedDate.Date + currentShift.EndTime;

            // Handle overnight shifts
            if (currentEnd <= currentStart)
            {
                currentEnd = currentEnd.AddDays(1);
            }

            foreach (var shift in existingShifts)
            {
                DateTime existingStart =
                    shift.AssignedDate.Date + shift.StartTime;

                DateTime existingEnd =
                    shift.AssignedDate.Date + shift.EndTime;

                if (existingEnd <= existingStart)
                {
                    existingEnd = existingEnd.AddDays(1);
                }

                // Overlapping shifts
                if (currentStart < existingEnd &&
                    currentEnd > existingStart)
                {
                    return true;
                }

                double restAfterExisting =
                    (currentStart - existingEnd).TotalHours;

                double restBeforeExisting =
                    (existingStart - currentEnd).TotalHours;

                if ((restAfterExisting >= 0 &&
                     restAfterExisting < 11) ||
                    (restBeforeExisting >= 0 &&
                     restBeforeExisting < 11))
                {
                    return true;
                }
            }

            return false;
        }


        private bool HasRequiredSkill(List<string> skills, ShiftAssignment shift)
        {

            if (string.IsNullOrWhiteSpace(shift.Role))
                return false;

            return skills != null &&
                   skills.Count > 0 &&
                   skills.Any(skill =>
                       skill != null &&
                       skill.Trim().Equals(shift.Role.Trim(), StringComparison.OrdinalIgnoreCase));

        }

        private async Task UpdateShiftStatus(ShiftAssignment shift, int userId, DateTime targetDate, bool hasAnyViolation)
        {
            if (hasAnyViolation)
            {
                shift.Status = ShiftAssignmentStatus.Cancelled;
                return;
            }

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
                shift.Status = ShiftAssignmentStatus.Assigned;
            }
        }
    }
}
