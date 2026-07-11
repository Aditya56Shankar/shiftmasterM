using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.SchedulingService.Enums;

namespace ShiftMaster.SchedulingService.Models
{
    public class SwapRequest
    {
        [Key] public int SwapID { get; set; }
        [Required, MaxLength(500)] public string Reason { get; set; } = null!;
        [Required] public ApprovalStatus Status { get; set; }

        [Required] public int RequesterUserID { get; set; } // Foreign Key to Identity Service User
        [Required] public int TargetUserID { get; set; } // Foreign Key to Identity Service User

        [Required]
        [ForeignKey(nameof(OriginalAssignment))]
        public int OriginalAssignmentID { get; set; }
        public ShiftAssignment OriginalAssignment { get; set; } = null!;

        [ForeignKey(nameof(ProposedAssignment))]
        public int? ProposedAssignmentID { get; set; }
        public ShiftAssignment? ProposedAssignment { get; set; }

        public int? ApprovedByID { get; set; } // Foreign Key to Identity Service User (Supervisor)
    }
}
