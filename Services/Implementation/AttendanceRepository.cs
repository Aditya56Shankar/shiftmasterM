using Data.Context;
using Domain.Enums;
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

    public async Task<AttendanceRecord> CreateAttendanceAsync(AttendanceRecord record)
    {
        // ✅ Load only Assignment (NO Include)
        var assignment = await _context.Set<ShiftAssignment>()
            .FirstOrDefaultAsync(a => a.AssignmentID == record.AssignmentID);

        if (assignment == null) return null;

        var shiftStart = assignment.StartTime;
        var shiftEnd = assignment.EndTime;

        var clockIn = record.ClockIn;
        var clockOut = record.ClockOut;

        // ✅ Calculate work hours
        if (clockIn.HasValue && clockOut.HasValue)
        {
            var totalMinutes = (clockOut.Value - clockIn.Value).TotalMinutes;
            totalMinutes -= record.BreakMinutesTaken;

            record.ActualHoursWorked = (decimal)(totalMinutes / 60.0);
        }
        else
        {
            record.ActualHoursWorked = 0;
        }

        // ✅ Detect Night Shift
        bool isNightShift = shiftEnd < shiftStart;

        // ✅ Status Logic
        if (!clockIn.HasValue)
        {
            record.Status = AttendanceStatus.Absent;
        }
        else
        {
            var clockInTime = clockIn.Value.TimeOfDay;
            var clockOutTime = clockOut?.TimeOfDay;

            // ✅ 10 min grace
            var graceStart = shiftStart.Add(TimeSpan.FromMinutes(10));

            if (!isNightShift)
            {
                // ✅ Day shift
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
                // ✅ Night shift
                if (clockInTime > graceStart)
                {
                    record.Status = AttendanceStatus.Late;
                }
                else
                {
                    record.Status = AttendanceStatus.Present;
                }
            }
        }

        // ✅ Variance (8 hrs standard)
        record.VarianceMinutes = (int)((record.ActualHoursWorked - 8) * 60);

        _context.AttendanceRecords.Add(record);
        await _context.SaveChangesAsync();

        return record;
    }

    public async Task<TimesheetSummary> CreateTimesheetAsync(int userId, DateTime weekStart)
    {
        var weekEnd = weekStart.AddDays(7);

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
}
