using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class LeaveBlockResponseDto
    {
        public int LeaveBlockID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
