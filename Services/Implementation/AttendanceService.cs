using Domain.Enums;
using Services.Interfaces;
using shiftmaster.models;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _repo;

    public AttendanceService(IAttendanceRepository repo)
    {
        _repo = repo;
    }

    public async Task<AttendanceRecord?> CreateAttendanceAsync(AttendanceRecord record)
    {
        var assignment = await _repo.GetAssignmentAsync(record.AssignmentID);
        if (assignment == null) return null;

        var shiftStart = assignment.StartTime;
        var shiftEnd = assignment.EndTime;

        var clockIn = record.ClockIn;
        var clockOut = record.ClockOut;

        // Work hours calculation
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

        bool isNightShift = shiftEnd < shiftStart;

        if (!clockIn.HasValue)
        {
            record.Status = AttendanceStatus.Absent;
        }
        else
        {
            var clockInTime = clockIn.Value.TimeOfDay;
            var clockOutTime = clockOut?.TimeOfDay;
            var graceStart = shiftStart.Add(TimeSpan.FromMinutes(10));

            if (!isNightShift)
            {
                if (clockInTime > graceStart)
                    record.Status = AttendanceStatus.Late;
                else if (clockOut.HasValue && clockOutTime < shiftEnd)
                    record.Status = AttendanceStatus.EarlyLeave;
                else
                    record.Status = AttendanceStatus.Present;
            }
            else
            {
                record.Status = clockInTime > graceStart
                    ? AttendanceStatus.Late
                    : AttendanceStatus.Present;
            }
        }

        record.VarianceMinutes = (int)((record.ActualHoursWorked - 8) * 60);

        await _repo.AddAttendanceAsync(record);
        return record;
    }

    public async Task<TimesheetSummary> CreateTimesheetAsync(int userId, DateTime weekStart)
    {
        var existing = await _repo.GetTimesheet(userId, weekStart);

        if (existing != null)
            throw new InvalidOperationException("Timesheet already exists");

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

    public async Task<TimesheetSummary?> UpdateTimesheetStatusAsync(
    int timesheetId,
    TimesheetStatus newStatus,
    int userId)
    {
        var timesheet = await _repo.GetTimesheetById(timesheetId);

        if (timesheet == null) return null;

        // ❌ Remove this (it blocks payroll step)
        // if (timesheet.Status == TimesheetStatus.Approved)
        //     throw new InvalidOperationException("Already approved");

        if (timesheet.Status == newStatus)
            throw new InvalidOperationException("Duplicate status update");

        switch (newStatus)
        {
            // ✅ Supervisor approves ONLY if Submitted
            case TimesheetStatus.Approved:
                if (timesheet.Status != TimesheetStatus.Submitted)
                    throw new InvalidOperationException(
                        "Only submitted timesheets can be approved"
                    );

                timesheet.ApprovedByID = userId;
                break;

            // ✅ Payroll sends ONLY if Approved
            case TimesheetStatus.SentToPayroll:
                if (timesheet.Status != TimesheetStatus.Approved)
                    throw new InvalidOperationException(
                        "Only approved timesheets can be sent to payroll"
                    );

                timesheet.ApprovedByID = userId; // optional field
                break;

            default:
                throw new InvalidOperationException("Invalid status transition");
        }

        timesheet.Status = newStatus;

        await _repo.SaveChangesAsync();

        return timesheet;
    }
}