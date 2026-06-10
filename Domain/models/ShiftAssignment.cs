using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class ShiftAssignment : IMustHaveTenant
    {
        [Key] public int AssignmentID { get; set; }
        [Required] public DateTime AssignedDate { get; set; }
        [Required] public TimeSpan StartTime { get; set; }
        [Required] public TimeSpan EndTime { get; set; }
        [Required, MaxLength(100)] public string Role { get; set; }
        [Required] public ShiftAssignmentStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int RosterID { get; set; }
        public WeeklyRoster Roster { get; set; }

        [Required] public int UserID { get; set; }
        public User Employee { get; set; }

        [Required] public int ShiftPatternID { get; set; }
        public ShiftPattern Pattern { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        // 1-to-1 Relationship
        public AttendanceRecord Attendance { get; set; }
    }
}
