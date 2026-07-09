using Data.Context;
using shiftmaster.models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly ApplicationDbContext _context;

    public AttendanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftAssignment?> GetAssignmentAsync(int assignmentId)
    {
        return await _context.Set<ShiftAssignment>()
            .Include(a => a.Pattern)
            .FirstOrDefaultAsync(a => a.AssignmentID == assignmentId);
    }

    public async Task<bool> AttendanceExistsForAssignmentAsync(int assignmentId)
    {
        return await _context.AttendanceRecords.AnyAsync(a => a.AssignmentID == assignmentId);
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users.AnyAsync(u => u.UserID == userId);
    }

    public async Task<bool> WorkLocationExistsAsync(int locationId)
    {
        return await _context.WorkLocations.AnyAsync(w => w.LocationID == locationId);
    }

    public async Task<AttendanceRecord?> GetAttendanceByIdAsync(int attendanceId)
    {
        return await _context.AttendanceRecords
            .Include(a => a.Assignment)
                .ThenInclude(a => a.Pattern)
            .FirstOrDefaultAsync(a => a.AttendanceID == attendanceId);
    }

    public async Task AddAttendanceAsync(AttendanceRecord record)
    {
        _context.AttendanceRecords.Add(record);
        await _context.SaveChangesAsync();
    }

    public async Task<AttendanceRecord?> GetAttendanceForUserDateAsync(int userId, DateTime workDate)
    {
        return await _context.AttendanceRecords
            .Include(a => a.Assignment)
                .ThenInclude(a => a.Pattern)
            .FirstOrDefaultAsync(x => x.UserID == userId && x.WorkDate.Date == workDate.Date);
    }

    public async Task<List<AttendanceRecord>> GetAttendanceForUserWeekAsync(int userId, DateTime weekStart)
    {
        var weekEnd = weekStart.Date.AddDays(7);

        return await _context.AttendanceRecords
            .Include(a => a.Assignment)
                .ThenInclude(a => a.Pattern)
            .Where(x => x.UserID == userId && x.WorkDate >= weekStart.Date && x.WorkDate < weekEnd)
            .OrderBy(x => x.WorkDate)
            .ToListAsync();
    }

    public async Task<List<AttendanceRecord>> GetAttendanceByLocationDateAsync(int locationId, DateTime workDate)
    {
        return await _context.AttendanceRecords
            .Include(a => a.Assignment)
                .ThenInclude(a => a.Pattern)
            .Where(x => x.WorkDate.Date == workDate.Date && x.Assignment.Pattern.LocationID == locationId)
            .OrderBy(x => x.UserID)
            .ToListAsync();
    }

    public async Task<List<AttendanceRecord>> GetFlaggedAttendanceByLocationDateAsync(int locationId, DateTime workDate)
    {
        return await _context.AttendanceRecords
            .Include(a => a.Assignment)
                .ThenInclude(a => a.Pattern)
            .Where(x => x.WorkDate.Date == workDate.Date
                && x.Assignment.Pattern.LocationID == locationId
                && (x.Status == AttendanceStatus.Late
                    || x.Status == AttendanceStatus.EarlyLeave
                    || x.Status == AttendanceStatus.Absent))
            .OrderBy(x => x.UserID)
            .ToListAsync();
    }

    public async Task<LeaveBlock?> GetActiveLeaveForUserDateAsync(int userId, DateTime workDate)
    {
        return await _context.LeaveBlocks
            .FirstOrDefaultAsync(x => x.UserID == userId
                && x.Status == LeaveStatus.Active
                && x.StartDate.Date <= workDate.Date
                && x.EndDate.Date >= workDate.Date);
    }

    public async Task<List<AttendanceRecord>> GetWeeklyRecords(int userId, DateTime weekStart)
    {
        var weekEnd = weekStart.AddDays(7);

        return await _context.AttendanceRecords
            .Where(x => x.UserID == userId &&
                        x.WorkDate >= weekStart &&
                        x.WorkDate < weekEnd)
            .ToListAsync();
    }

    public async Task<TimesheetSummary?> GetTimesheet(int userId, DateTime weekStart)
    {
        return await _context.TimesheetSummaries
            .FirstOrDefaultAsync(x => x.UserID == userId &&
                                     x.WeekStartDate == weekStart);
    }

    public async Task<TimesheetSummary?> GetTimesheetById(int id)
    {
        return await _context.TimesheetSummaries
            .FirstOrDefaultAsync(x => x.TimesheetID == id);
    }

    public async Task<List<TimesheetSummary>> GetTimesheetsByStatusAndLocationAsync(TimesheetStatus status, int locationId)
    {
        return await _context.TimesheetSummaries
            .Where(ts => ts.Status == status
                && _context.AttendanceRecords.Any(ar => ar.UserID == ts.UserID
                    && ar.WorkDate >= ts.WeekStartDate
                    && ar.WorkDate < ts.WeekStartDate.AddDays(7)
                    && ar.Assignment.Pattern.LocationID == locationId))
            .OrderBy(ts => ts.WeekStartDate)
            .ToListAsync();
    }

    public async Task<List<TimesheetSummary>> GetTimesheetsByStatusAndWeekAsync(TimesheetStatus status, DateTime weekStart)
    {
        return await _context.TimesheetSummaries
            .Where(ts => ts.Status == status && ts.WeekStartDate.Date == weekStart.Date)
            .OrderBy(ts => ts.UserID)
            .ToListAsync();
    }

    public async Task AddTimesheetAsync(TimesheetSummary timesheet)
    {
        _context.TimesheetSummaries.Add(timesheet);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}