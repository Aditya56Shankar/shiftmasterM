using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.SchedulingService.Domain.Enums;

namespace ShiftMaster.SchedulingService.Domain.Models
{
    public class CoverAssignment
    {
        [Key] public int CoverID { get; set; }
        [Required] public CoverType CoverType { get; set; }
        [Required] public bool OvertimeApplicable { get; set; }
        [Required] public CoverStatus Status { get; set; }

        [Required]
        [ForeignKey(nameof(OriginalAssignment))]
        public int OriginalAssignmentID { get; set; }
        public ShiftAssignment OriginalAssignment { get; set; } = null!;

        [Required] public int CoveringUserID { get; set; } // Foreign Key to Identity Service User
        [Required] public int AssignedByID { get; set; } // Foreign Key to Identity Service User (Supervisor)
    }
}
