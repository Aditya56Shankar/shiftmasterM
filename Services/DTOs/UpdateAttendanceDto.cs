using System;
using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateAttendanceDto
    {
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }

        [Range(0, 1440)]
        public int BreakMinutesTaken { get; set; }
    }
}