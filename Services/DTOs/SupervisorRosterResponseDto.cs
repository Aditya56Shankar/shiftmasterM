using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class SupervisorRosterResponseDto
    {
        public int RosterID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string Status { get; set; } // Represented as a string description for frontend clarity
        public int? CreatedByID { get; set; }
        public DateTime? PublishedDate { get; set; }

        // Array collections nested for the management grid
        public ICollection<SupervisorAssignmentViewDto> ShiftAssignments { get; set; } = new List<SupervisorAssignmentViewDto>();
        public ICollection<ViolationViewDto> Violations { get; set; } = new List<ViolationViewDto>();
    }

    public class SupervisorAssignmentViewDto
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; } // Flattened property so the UI can quickly render names
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }

    public class ViolationViewDto
    {
        public int ViolationID { get; set; }
        public int UserID { get; set; }
        public string ViolationType { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
    }
}
