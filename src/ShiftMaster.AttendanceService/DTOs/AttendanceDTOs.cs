using System;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.AttendanceService.DTOs
{
    public class CreateAttendanceDto
    {
        [Range(1, int.MaxValue)]
        public int AssignmentID { get; set; }

        public DateTime WorkDate { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }

        [Range(0, 1440)]
        public int BreakMinutesTaken { get; set; }
    }

    public class UpdateAttendanceDto
    {
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }

        [Range(0, 1440)]
        public int BreakMinutesTaken { get; set; }
    }

    public class AttendanceDtoResponse
    {
        public int AttendanceID { get; set; }
        public int UserID { get; set; }
        public int AssignmentID { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public int BreakMinutesTaken { get; set; }
        public decimal ActualHoursWorked { get; set; }
        public int VarianceMinutes { get; set; }
        public string Status { get; set; } = null!;
    }

    public class CreateTimesheetDto
    {
        public DateTime WeekStartDate { get; set; }
    }

    public class TimesheetDtoResponse
    {
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public decimal TotalHours { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal PublicHolidayHours { get; set; }
        public string Status { get; set; } = null!;
        public int? SupervisorApprovedByID { get; set; }
        public int? HrApprovedByID { get; set; }
        public int? PayrollProcessedByID { get; set; }
    }

    public class CreateOvertimeDto
    {
        [Range(1, int.MaxValue)]
        public int UserID { get; set; }

        [Range(1, int.MaxValue)]
        public int AuthorisedByID { get; set; }

        public DateTime WeekStartDate { get; set; }

        [Range(typeof(decimal), "0.1", "79228162514264337593543950335")]
        public decimal PlannedOTHours { get; set; }

        [Range(typeof(decimal), "0.1", "79228162514264337593543950335")]
        public decimal ActualOTHours { get; set; }

        public string OTType { get; set; } = null!;
    }

    public class OvertimeAuthorisationDto
    {
        public int OTID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateTime WeekStartDate { get; set; }
        public decimal PlannedOTHours { get; set; }
        public decimal ActualOTHours { get; set; }
        public string OTType { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class OvertimeAuthorisationResponseDto
    {
        public int OTID { get; set; }
        public int UserID { get; set; }
        public int AuthorisedByID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public decimal PlannedOTHours { get; set; }
        public decimal ActualOTHours { get; set; }
        public string OTType { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class AuthoriseOvertimeDto
    {
        [Range(1, int.MaxValue)]
        public int AuthorisedByID { get; set; }
        public bool Approved { get; set; }
    }
}
