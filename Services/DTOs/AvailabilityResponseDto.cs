using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

    public class AvailabilityResponseDto
    {
        public int AvailabilityID { get; set; }
        public int UserID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string Status { get; set; }
    }

}
