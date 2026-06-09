using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class SwapRequest
    {
        [Key] public int SwapID { get; set; }
        [Required, MaxLength(500)] public string Reason { get; set; }
        [Required] public ApprovalStatus Status { get; set; }

        // Complex Foreign Keys & Navigation
        [Required] public int RequesterUserID { get; set; }
        public User Requester { get; set; }

        [Required] public int TargetUserID { get; set; }
        public User Target { get; set; }

        [Required] public int OriginalAssignmentID { get; set; }
        public ShiftAssignment OriginalAssignment { get; set; }

        public int? ProposedAssignmentID { get; set; }
        public ShiftAssignment ProposedAssignment { get; set; }

        public int? ApprovedByID { get; set; }
        public User ApprovedBy { get; set; }
    }
}
