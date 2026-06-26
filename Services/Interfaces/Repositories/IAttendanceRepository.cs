using shiftmaster.models;

public interface IAttendanceRepository
{
    // Attendance
    Task<ShiftAssignment?> GetAssignmentAsync(int assignmentId);
    Task AddAttendanceAsync(AttendanceRecord record);

    // Attendance Queries
    Task<List<AttendanceRecord>> GetWeeklyRecords(int userId, DateTime weekStart);

    // Timesheet
    Task<TimesheetSummary?> GetTimesheet(int userId, DateTime weekStart);
    Task<TimesheetSummary?> GetTimesheetById(int id);
    Task AddTimesheetAsync(TimesheetSummary timesheet);

    // Save changes
    Task SaveChangesAsync();
}