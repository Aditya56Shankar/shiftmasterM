using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class LeaveBlock
    {
        [Key] public int LeaveBlockID { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public LeaveReason Reason { get; set; }
        [Required] public LeaveStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required]
        [ForeignKey(nameof(Employee))] public int UserID { get; set; }
        public User Employee { get; set; }

        [ForeignKey(nameof(ApprovedBy))]
        public int? ApprovedByID { get; set; }
        public User ApprovedBy { get; set; }
    }
}
