using shiftmaster.models;
using Domain.Enums;

public interface IAttendanceRepository
{
    // Attendance
    Task<ShiftAssignment?> GetAssignmentAsync(int assignmentId);
    Task<bool> AttendanceExistsForAssignmentAsync(int assignmentId);
    Task<AttendanceRecord?> GetAttendanceByIdAsync(int attendanceId);
    Task AddAttendanceAsync(AttendanceRecord record);
    Task<bool> UserExistsAsync(int userId);
    Task<bool> WorkLocationExistsAsync(int locationId);
    Task<AttendanceRecord?> GetAttendanceForUserDateAsync(int userId, DateTime workDate);
    Task<List<AttendanceRecord>> GetAttendanceForUserWeekAsync(int userId, DateTime weekStart);
    Task<List<AttendanceRecord>> GetAttendanceByLocationDateAsync(int locationId, DateTime workDate);
    Task<List<AttendanceRecord>> GetFlaggedAttendanceByLocationDateAsync(int locationId, DateTime workDate);
    Task<LeaveBlock?> GetActiveLeaveForUserDateAsync(int userId, DateTime workDate);

    // Attendance Queries
    Task<List<AttendanceRecord>> GetWeeklyRecords(int userId, DateTime weekStart);

    // Timesheet
    Task<TimesheetSummary?> GetTimesheet(int userId, DateTime weekStart);
    Task<TimesheetSummary?> GetTimesheetById(int id);
    Task<List<TimesheetSummary>> GetTimesheetsByStatusAndLocationAsync(TimesheetStatus status, int locationId);
    Task<List<TimesheetSummary>> GetTimesheetsByStatusAndWeekAsync(TimesheetStatus status, DateTime weekStart);
    Task AddTimesheetAsync(TimesheetSummary timesheet);

    // Save changes
    Task SaveChangesAsync();
}