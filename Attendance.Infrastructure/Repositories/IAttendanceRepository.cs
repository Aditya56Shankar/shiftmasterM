using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.AttendanceService.Domain.Models;
using ShiftMaster.AttendanceService.Domain.Enums;

namespace ShiftMaster.AttendanceService.Infrastructure.Repositories
{
    public interface IAttendanceRepository
    {
        // Attendance
        Task<bool> AttendanceExistsForAssignmentAsync(int assignmentId);
        Task<AttendanceRecord?> GetAttendanceByIdAsync(int attendanceId);
        Task AddAttendanceAsync(AttendanceRecord record);
        Task<AttendanceRecord?> GetAttendanceForUserDateAsync(int userId, DateTime workDate);
        Task<List<AttendanceRecord>> GetAttendanceForUserWeekAsync(int userId, DateTime weekStart);
        Task<List<AttendanceRecord>> GetAttendanceByLocationDateAsync(int locationId, DateTime workDate);
        Task<List<AttendanceRecord>> GetFlaggedAttendanceByLocationDateAsync(int locationId, DateTime workDate);

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
}
