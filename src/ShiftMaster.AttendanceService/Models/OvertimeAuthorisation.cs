using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.AttendanceService.Enums;

namespace ShiftMaster.AttendanceService.Models
{
    public class OvertimeAuthorisation
    {
        [Key]
        public int OTID { get; set; }

        [Required]
        public DateTime WeekStartDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PlannedOTHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ActualOTHours { get; set; }

        [Required]
        public OTType OTType { get; set; }

        [Required]
        public ApprovalStatus Status { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int AuthorisedByID { get; set; }
    }
}
