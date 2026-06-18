using shiftmaster.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{

    public interface IAttendanceRepository
    {
        Task<AttendanceRecord> CreateAttendanceAsync(AttendanceRecord record);
        Task<TimesheetSummary> CreateTimesheetAsync(int userId, DateTime weekStart);
    }

}
