using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.SchedulingService.Domain.Enums;

namespace ShiftMaster.SchedulingService.Domain.Models
{
    public class ShiftAssignment
    {
        [Key] public int AssignmentID { get; set; }
        [Required] public DateTime AssignedDate { get; set; }
        [Required] public TimeSpan StartTime { get; set; }
        [Required] public TimeSpan EndTime { get; set; }
        [Required, MaxLength(100)] public string Role { get; set; } = null!;
        [Required] public ShiftAssignmentStatus Status { get; set; }

        [Required]
        [ForeignKey(nameof(Roster))]
        public int RosterID { get; set; }
        public WeeklyRoster Roster { get; set; } = null!;

        [Required] public int UserID { get; set; } // Foreign Key to Identity Service User

        [Required]
        [ForeignKey(nameof(Pattern))]
        public int ShiftPatternID { get; set; }
        public ShiftPattern Pattern { get; set; } = null!;
    }
}
