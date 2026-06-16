using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

    public class AvailabilityRequestDto
    {
        public int UserID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string AvailableDays { get; set; }
        public string PreferredShiftType { get; set; }
        public decimal MaxHoursPerWeek { get; set; }
    }

}
