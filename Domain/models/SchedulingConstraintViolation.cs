using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class SchedulingConstraintViolation : IMustHaveTenant
    {
        [Key] public int ViolationID { get; set; }
        [Required] public ViolationType ViolationType { get; set; }
        [Required] public SeverityLevel Severity { get; set; }
        [Required] public ViolationStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int RosterID { get; set; }
        public WeeklyRoster Roster { get; set; }

        [Required] public int UserID { get; set; }
        public User Employee { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
