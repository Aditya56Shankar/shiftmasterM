using Data.Context;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using shiftmaster.models;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly ApplicationDbContext _context;

    public AttendanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ CREATE ATTENDANCE
    public async Task<AttendanceRecord> CreateAttendanceAsync(AttendanceRecord record)
    {
        var assignment = await _context.Set<ShiftAssignment>()
            .FirstOrDefaultAsync(a => a.AssignmentID == record.AssignmentID);

        if (assignment == null) return null;

        var shiftStart = assignment.StartTime;
        var shiftEnd = assignment.EndTime;

        var clockIn = record.ClockIn;
        var clockOut = record.ClockOut;

        // ✅ Safe work hours calc
        if (clockIn.HasValue && clockOut.HasValue)
        {
            var totalMinutes = (clockOut.Value - clockIn.Value).TotalMinutes;

            // ✅ Fix negative (night shift)
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
                {
                    record.Status = AttendanceStatus.Late;
                }
                else if (clockOut.HasValue && clockOutTime < shiftEnd)
                {
                    record.Status = AttendanceStatus.EarlyLeave;
                }
                else
                {
                    record.Status = AttendanceStatus.Present;
                }
            }
            else
            {
                record.Status = clockInTime > graceStart
                    ? AttendanceStatus.Late
                    : AttendanceStatus.Present;
            }
        }

        record.VarianceMinutes = (int)((record.ActualHoursWorked - 8) * 60);

        _context.AttendanceRecords.Add(record);
        await _context.SaveChangesAsync();

        return record;
    }

    //  CREATE TIMESHEET (POST → Submitted)
    public async Task<TimesheetSummary> CreateTimesheetAsync(int userId, DateTime weekStart)
    {
        var weekEnd = weekStart.AddDays(7);

        //  Prevent duplicate timesheet
        var existing = await _context.TimesheetSummaries
            .FirstOrDefaultAsync(x => x.UserID == userId &&
                                      x.WeekStartDate == weekStart);

        if (existing != null)
            throw new InvalidOperationException("Timesheet already exists for this week");

        var records = await _context.AttendanceRecords
            .Where(x => x.UserID == userId &&
                        x.WorkDate >= weekStart &&
                        x.WorkDate < weekEnd)
            .ToListAsync();

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

        _context.TimesheetSummaries.Add(timesheet);
        await _context.SaveChangesAsync();

        return timesheet;
    }

    //  UPDATE STATUS
    public async Task<TimesheetSummary?> UpdateTimesheetStatusAsync(
    int timesheetId,
    TimesheetStatus newStatus,
    int userId)
    {
        var timesheet = await _context.TimesheetSummaries
            .FirstOrDefaultAsync(x => x.TimesheetID == timesheetId);

        if (timesheet == null)
            return null;

        // ✅ Prevent updating if already final
        if (timesheet.Status == TimesheetStatus.Approved)
            throw new InvalidOperationException("Timesheet already approved and locked");

        // ✅ Prevent duplicate updates
        if (timesheet.Status == newStatus)
            throw new InvalidOperationException($"Timesheet already in '{newStatus}' state");

        // ✅ Workflow validation
        switch (newStatus)
        {
            case TimesheetStatus.SentToPayroll:
                if (timesheet.Status != TimesheetStatus.Submitted)
                    throw new InvalidOperationException(
                        $"Cannot move to SentToPayroll from {timesheet.Status}");

                // ✅ Track Supervisor who sent it
                timesheet.ApprovedByID = userId;
                break;

            case TimesheetStatus.Approved:
                if (timesheet.Status != TimesheetStatus.SentToPayroll)
                    throw new InvalidOperationException(
                        $"Cannot approve from {timesheet.Status}");

                // ✅ Track Payroll approver
                timesheet.ApprovedByID = userId;
                break;

            default:
                throw new InvalidOperationException("Invalid status transition");
        }

        // ✅ Update status
        timesheet.Status = newStatus;

        await _context.SaveChangesAsync();

        return timesheet;
    }
}