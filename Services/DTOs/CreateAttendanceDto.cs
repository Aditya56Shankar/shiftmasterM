using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

    public class CreateAttendanceDto
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public int BreakMinutesTaken { get; set; }
    }

}
