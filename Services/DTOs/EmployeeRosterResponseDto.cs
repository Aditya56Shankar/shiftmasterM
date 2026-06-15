using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class EmployeeRosterResponseDto
    {
        public int RosterID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }

        // Displays strictly confirmed personal working slots for the requested week
        public ICollection<EmployeeAssignmentViewDto> MyShifts { get; set; } = new List<EmployeeAssignmentViewDto>();
    }

    public class EmployeeAssignmentViewDto
    {
        public int AssignmentID { get; set; }
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; }
        public string LocationName { get; set; } // Helpful context for cross-location workers
    }
}
