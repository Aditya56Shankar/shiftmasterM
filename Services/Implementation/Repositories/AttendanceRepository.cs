using Data.Context;
using shiftmaster.models;
using Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
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
            .FirstOrDefaultAsync(a => a.AssignmentID == assignmentId);
    }

    public async Task AddAttendanceAsync(AttendanceRecord record)
    {
        _context.AttendanceRecords.Add(record);
        await _context.SaveChangesAsync();
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