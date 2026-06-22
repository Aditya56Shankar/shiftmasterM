using System;

namespace Services.DTOs
{
    public class ShiftPatternDto
    {
        public int PatternID { get; set; }
        public string PatternName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal DurationHours { get; set; }
        public int BreakMinutes { get; set; }
        public string ShiftType { get; set; }
        public int MinStaffingLevel { get; set; }
        public string Status { get; set; }
        public int LocationID { get; set; }
    }
}