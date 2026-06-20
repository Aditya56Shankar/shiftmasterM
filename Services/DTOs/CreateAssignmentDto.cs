using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.DTOs
{
    public class CreateAssignmentDto
    {
        [Required(ErrorMessage = "Roster ID container reference is required.")]
        public int RosterID { get; set; }

        [Required(ErrorMessage = "Target employee UserID is required.")]
        public int UserID { get; set; }

        public int? ShiftPatternID { get; set; }

        [Required(ErrorMessage = "Shift calendar date is required.")]
        public DateTime AssignedDate { get; set; }

        [Required(ErrorMessage = "Shift start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Shift end time is required.")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Assigned functional role is required.")]
        [StringLength(100, ErrorMessage = "Role cannot exceed 100 characters.")]
        public string Role { get; set; }
    }
}
