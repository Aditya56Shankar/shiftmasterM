using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class CoverAssignment
    {
        [Key] public int CoverID { get; set; }
        [Required] public CoverType CoverType { get; set; }
        [Required] public bool OvertimeApplicable { get; set; }
        [Required] public CoverStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int OriginalAssignmentID { get; set; }
        public ShiftAssignment OriginalAssignment { get; set; }

        [Required] public int CoveringUserID { get; set; }
        public User CoveringEmployee { get; set; }

        [Required] public int AssignedByID { get; set; }
        public User AssignedBy { get; set; }
    }
}
