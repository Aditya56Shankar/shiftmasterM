using System;
using System.Collections.Generic;
using System.Text;

namespace shiftMaster.Services.DTOs
{
    public class AssignmentResponseDto
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public int RosterID { get; set; }
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
