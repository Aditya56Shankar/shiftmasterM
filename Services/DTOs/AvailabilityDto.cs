using System;

namespace Services.DTOs
{
    public class AvailabilityDto
    {
        public int AvailabilityID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string AvailableDays { get; set; }
        public string PreferredShiftType { get; set; }
        public decimal MaxHoursPerWeek { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Status { get; set; }
    }
}