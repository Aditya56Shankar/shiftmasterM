using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

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
        public string Status { get; set; }
    }

}
