using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.SchedulingService.Domain.Enums;

namespace ShiftMaster.SchedulingService.Domain.Models
{
    public class SchedulingConstraintViolation
    {
        [Key] public int ViolationID { get; set; }
        [Required] public ViolationType ViolationType { get; set; }
        [Required] public SeverityLevel Severity { get; set; }
        [Required] public ViolationStatus Status { get; set; }

        [Required]
        [ForeignKey(nameof(Roster))]
        public int RosterID { get; set; }
        public WeeklyRoster Roster { get; set; } = null!;

        public int? UserID { get; set; } // Foreign Key to Identity Service User
    }
}
