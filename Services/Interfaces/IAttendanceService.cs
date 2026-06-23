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
            Task<TimesheetSummary> CreateTimesheetAsync(int userId, DateTime weekStart);
            Task<TimesheetSummary?> UpdateTimesheetStatusAsync(int timesheetId, TimesheetStatus newStatus, int userId);


    }

}
