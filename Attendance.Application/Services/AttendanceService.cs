using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShiftMaster.AttendanceService.Clients;
using ShiftMaster.AttendanceService.Applications.Interfaces;
using ShiftMaster.AttendanceService.Domain.Models;
using ShiftMaster.AttendanceService.Domain.Enums;
using ShiftMaster.AttendanceService.Infrastructure.Repositories;
using ShiftMaster.AttendanceService.Applications.Exceptions;

namespace ShiftMaster.AttendanceService.Applications.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _repo;
        private readonly ISchedulingClient _schedulingClient;
        private readonly IIdentityClient _identityClient;
        private readonly IEmployeeClient _employeeClient;

        public AttendanceService(
            IAttendanceRepository repo,
            ISchedulingClient schedulingClient,
            IIdentityClient identityClient,
            IEmployeeClient employeeClient)
        {
            _repo = repo;
            _schedulingClient = schedulingClient;
            _identityClient = identityClient;
            _employeeClient = employeeClient;
        }

        public async Task<AttendanceRecord?> CreateAttendanceAsync(AttendanceRecord record)
        {
            if (record.BreakMinutesTaken < 0)
            {
                throw new InvalidWorkflowStateException("BreakMinutesTaken cannot be negative.");
            }

            var assignment = await _schedulingClient.GetAssignmentAsync(record.AssignmentID);
            if (assignment == null)
            {
                throw new ResourceNotFoundException($"Assignment with ID {record.AssignmentID} not found.");
            }

            var duplicateAttendance = await _repo.AttendanceExistsForAssignmentAsync(record.AssignmentID);
            if (duplicateAttendance)
            {
                throw new InvalidWorkflowStateException($"Attendance already exists for assignment ID {record.AssignmentID}.");
            }

            if (assignment.UserID != record.UserID)
            {
                throw new InvalidWorkflowStateException("You can record attendance only for your own assignment.");
            }

            record.LocationID = assignment.LocationID;

            ApplyAttendanceMetrics(record, assignment);

            await _repo.AddAttendanceAsync(record);
            return record;
        }

        public async Task<AttendanceRecord?> GetAttendanceByIdAsync(int attendanceId)
        {
            return await _repo.GetAttendanceByIdAsync(attendanceId);
        }

        public async Task<AttendanceRecord?> GetAttendanceForUserDateAsync(int userId, DateTime workDate)
        {
            return await _repo.GetAttendanceForUserDateAsync(userId, workDate);
        }

        public async Task<List<AttendanceRecord>> GetAttendanceForUserWeekAsync(int userId, DateTime weekStart)
        {
            return await _repo.GetAttendanceForUserWeekAsync(userId, weekStart);
        }

        public async Task<List<AttendanceRecord>> GetAttendanceByLocationDateAsync(int locationId, DateTime workDate)
        {
            var locationExists = await _identityClient.WorkLocationExistsAsync(locationId);
            if (!locationExists)
            {
                throw new ResourceNotFoundException($"Location with ID {locationId} not found.");
            }

            return await _repo.GetAttendanceByLocationDateAsync(locationId, workDate);
        }

        public async Task<List<AttendanceRecord>> GetFlaggedAttendanceByLocationDateAsync(int locationId, DateTime workDate)
        {
            var locationExists = await _identityClient.WorkLocationExistsAsync(locationId);
            if (!locationExists)
            {
                throw new ResourceNotFoundException($"Location with ID {locationId} not found.");
            }

            return await _repo.GetFlaggedAttendanceByLocationDateAsync(locationId, workDate);
        }

        public async Task<AttendanceRecord?> ExcuseAttendanceAsync(int attendanceId)
        {
            var record = await _repo.GetAttendanceByIdAsync(attendanceId);
            if (record == null)
            {
                return null;
            }

            if (record.Status == AttendanceStatus.Present)
            {
                throw new InvalidWorkflowStateException("Cannot excuse already Present attendance.");
            }

            if (record.Status == AttendanceStatus.Excused)
            {
                throw new InvalidWorkflowStateException("Attendance is already marked as Excused.");
            }

            record.Status = AttendanceStatus.Excused;
            await _repo.SaveChangesAsync();
            return record;
        }

        public async Task<AttendanceRecord?> CorrectAttendanceAsync(int attendanceId, DateTime? clockIn, DateTime? clockOut, int breakMinutesTaken)
        {
            if (breakMinutesTaken < 0)
            {
                throw new InvalidWorkflowStateException("BreakMinutesTaken cannot be negative.");
            }

            var record = await _repo.GetAttendanceByIdAsync(attendanceId);
            if (record == null)
            {
                return null;
            }

            var assignment = await _schedulingClient.GetAssignmentAsync(record.AssignmentID);
            if (assignment == null)
            {
                return null;
            }

            record.ClockIn = clockIn;
            record.ClockOut = clockOut;
            record.BreakMinutesTaken = breakMinutesTaken;

            ApplyAttendanceMetrics(record, assignment);

            await _repo.SaveChangesAsync();
            return record;
        }

        private void ApplyAttendanceMetrics(AttendanceRecord record, EmployeeShiftShortDto assignment)
        {
            var shiftStart = assignment.StartTime;
            var shiftEnd = assignment.EndTime;

            var clockIn = record.ClockIn;
            var clockOut = record.ClockOut;

            if (clockIn.HasValue && clockOut.HasValue)
            {
                var totalMinutes = (clockOut.Value - clockIn.Value).TotalMinutes;

                if (totalMinutes < 0)
                    totalMinutes += 24 * 60;

                totalMinutes -= record.BreakMinutesTaken;

                if (totalMinutes < 0)
                    totalMinutes = 0;

                record.ActualHoursWorked = (decimal)(totalMinutes / 60.0);
            }
            else
            {
                record.ActualHoursWorked = 0;
            }

            var status = DetermineAttendanceStatus(record, assignment);
            record.Status = status;

            var scheduledHours = assignment.DurationHours;
            record.VarianceMinutes = (int)((record.ActualHoursWorked - scheduledHours) * 60);
        }

        private AttendanceStatus DetermineAttendanceStatus(AttendanceRecord record, EmployeeShiftShortDto assignment)
        {
            var clockIn = record.ClockIn;
            var clockOut = record.ClockOut;

            if (!clockIn.HasValue)
            {
                var leave = _employeeClient.HasActiveLeaveOnDateAsync(record.UserID, record.WorkDate)
                    .GetAwaiter()
                    .GetResult();

                return leave ? AttendanceStatus.Excused : AttendanceStatus.Absent;
            }

            var shiftStart = assignment.StartTime;
            var shiftEnd = assignment.EndTime;
            var isNightShift = shiftEnd <= shiftStart;
            var scheduledStart = record.WorkDate.Date.Add(shiftStart);
            var scheduledEnd = record.WorkDate.Date.Add(shiftEnd);

            if (isNightShift)
            {
                scheduledEnd = scheduledEnd.AddDays(1);
            }

            var normalizedClockIn = clockIn.Value;
            var normalizedClockOut = clockOut;

            if (isNightShift && normalizedClockIn < scheduledStart)
            {
                normalizedClockIn = normalizedClockIn.AddDays(1);
            }

            if (normalizedClockOut.HasValue && isNightShift && normalizedClockOut.Value < normalizedClockIn)
            {
                normalizedClockOut = normalizedClockOut.Value.AddDays(1);
            }

            var graceStart = scheduledStart.AddMinutes(10);

            if (normalizedClockIn > graceStart)
            {
                return AttendanceStatus.Late;
            }

            if (normalizedClockOut.HasValue && normalizedClockOut.Value < scheduledEnd)
            {
                return AttendanceStatus.EarlyLeave;
            }

            return AttendanceStatus.Present;
        }

        public async Task<TimesheetSummary> CreateTimesheetAsync(int userId, DateTime weekStart)
        {
            if (userId <= 0)
            {
                throw new InvalidWorkflowStateException("Invalid user ID.");
            }

            if (weekStart == default)
            {
                throw new InvalidWorkflowStateException("Invalid week start date.");
            }

            weekStart = weekStart.Date;

            var userExists = await _identityClient.UserExistsAsync(userId);
            if (!userExists)
            {
                throw new ResourceNotFoundException($"User with ID {userId} not found.");
            }

            var existing = await _repo.GetTimesheet(userId, weekStart);

            if (existing != null)
                throw new InvalidWorkflowStateException("Timesheet already exists.");

            var records = await _repo.GetWeeklyRecords(userId, weekStart);

            decimal totalHours = records.Sum(x => x.ActualHoursWorked);

            var timesheet = new TimesheetSummary
            {
                UserID = userId,
                WeekStartDate = weekStart,
                RegularHours = totalHours > 40 ? 40 : totalHours,
                OvertimeHours = totalHours > 40 ? totalHours - 40 : 0,
                PublicHolidayHours = 0,
                TotalHours = totalHours,
                Status = TimesheetStatus.Submitted
            };

            await _repo.AddTimesheetAsync(timesheet);
            return timesheet;
        }

        public async Task<TimesheetSummary?> UpdateTimesheetStatusAsync(int timesheetId, TimesheetStatus newStatus, int userId)
        {
            if (timesheetId <= 0)
            {
                throw new InvalidWorkflowStateException("Invalid timesheet ID.");
            }

            if (userId <= 0)
            {
                throw new InvalidWorkflowStateException("Invalid approver user ID.");
            }

            var timesheet = await _repo.GetTimesheetById(timesheetId);

            if (timesheet == null) return null;

            if (timesheet.Status == newStatus)
                throw new InvalidWorkflowStateException("Duplicate status update.");

            switch (newStatus)
            {
                case TimesheetStatus.SupervisorApproved:
                    if (timesheet.Status != TimesheetStatus.Submitted)
                        throw new InvalidWorkflowStateException("Only submitted timesheets can be supervisor approved");

                    timesheet.SupervisorApprovedByID = userId;
                    break;

                case TimesheetStatus.HrApproved:
                    if (timesheet.Status != TimesheetStatus.SupervisorApproved)
                        throw new InvalidWorkflowStateException("Only supervisor approved timesheets can be HR approved");

                    timesheet.HrApprovedByID = userId;
                    break;

                case TimesheetStatus.SentToPayroll:
                    if (timesheet.Status != TimesheetStatus.HrApproved)
                        throw new InvalidWorkflowStateException("Only HR approved timesheets can be sent to payroll");

                    timesheet.PayrollProcessedByID = userId;
                    break;

                case TimesheetStatus.Submitted:
                    if (timesheet.Status != TimesheetStatus.SupervisorApproved
                        && timesheet.Status != TimesheetStatus.HrApproved)
                        throw new InvalidWorkflowStateException("Only approved timesheets can be returned to submitted");

                    timesheet.SupervisorApprovedByID = null;
                    timesheet.HrApprovedByID = null;
                    break;

                default:
                    throw new InvalidWorkflowStateException("Invalid status transition.");
            }

            timesheet.Status = newStatus;

            await _repo.SaveChangesAsync();

            return timesheet;
        }

        public async Task<TimesheetSummary?> GetTimesheetAsync(int userId, DateTime weekStart)
        {
            if (userId <= 0)
            {
                throw new InvalidWorkflowStateException("Invalid user ID.");
            }

            if (weekStart == default)
            {
                throw new InvalidWorkflowStateException("Invalid week start date.");
            }

            return await _repo.GetTimesheet(userId, weekStart);
        }

        public async Task<TimesheetSummary?> GetTimesheetByIdAsync(int timesheetId)
        {
            if (timesheetId <= 0)
            {
                throw new InvalidWorkflowStateException("Invalid timesheet ID.");
            }

            return await _repo.GetTimesheetById(timesheetId);
        }

        public async Task<List<TimesheetSummary>> GetPendingSupervisorTimesheetsAsync(int locationId)
        {
            var locationExists = await _identityClient.WorkLocationExistsAsync(locationId);
            if (!locationExists)
            {
                throw new ResourceNotFoundException($"Location with ID {locationId} not found.");
            }

            return await _repo.GetTimesheetsByStatusAndLocationAsync(TimesheetStatus.Submitted, locationId);
        }

        public async Task<List<TimesheetSummary>> GetPendingHrTimesheetsAsync(int locationId)
        {
            var locationExists = await _identityClient.WorkLocationExistsAsync(locationId);
            if (!locationExists)
            {
                throw new ResourceNotFoundException($"Location with ID {locationId} not found.");
            }

            return await _repo.GetTimesheetsByStatusAndLocationAsync(TimesheetStatus.SupervisorApproved, locationId);
        }

        public async Task<List<TimesheetSummary>> GetApprovedTimesheetsForPayrollAsync(DateTime weekStart)
        {
            if (weekStart == default)
            {
                throw new InvalidWorkflowStateException("Invalid week start date.");
            }

            return await _repo.GetTimesheetsByStatusAndWeekAsync(TimesheetStatus.HrApproved, weekStart);
        }
    }
}
