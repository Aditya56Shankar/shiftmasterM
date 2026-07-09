using Domain.Enums;
using shiftmaster.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{

    public interface IAttendanceService
    {
            Task<AttendanceRecord?> CreateAttendanceAsync(AttendanceRecord record);
            Task<AttendanceRecord?> GetAttendanceByIdAsync(int attendanceId);
            Task<AttendanceRecord?> GetAttendanceForUserDateAsync(int userId, DateTime workDate);
            Task<List<AttendanceRecord>> GetAttendanceForUserWeekAsync(int userId, DateTime weekStart);
            Task<List<AttendanceRecord>> GetAttendanceByLocationDateAsync(int locationId, DateTime workDate);
            Task<List<AttendanceRecord>> GetFlaggedAttendanceByLocationDateAsync(int locationId, DateTime workDate);
            Task<AttendanceRecord?> ExcuseAttendanceAsync(int attendanceId);
            Task<AttendanceRecord?> CorrectAttendanceAsync(int attendanceId, DateTime? clockIn, DateTime? clockOut, int breakMinutesTaken);
            Task<TimesheetSummary> CreateTimesheetAsync(int userId, DateTime weekStart);
            Task<TimesheetSummary?> GetTimesheetAsync(int userId, DateTime weekStart);
            Task<TimesheetSummary?> GetTimesheetByIdAsync(int timesheetId);
            Task<List<TimesheetSummary>> GetPendingSupervisorTimesheetsAsync(int locationId);
            Task<List<TimesheetSummary>> GetPendingHrTimesheetsAsync(int locationId);
            Task<List<TimesheetSummary>> GetApprovedTimesheetsForPayrollAsync(DateTime weekStart);
            Task<TimesheetSummary?> UpdateTimesheetStatusAsync(int timesheetId, TimesheetStatus newStatus, int userId);
    }

}
