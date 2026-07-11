using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.AttendanceService.Domain.Enums;

namespace ShiftMaster.AttendanceService.Domain.Models
{
    public class TimesheetSummary
    {
        [Key]
        public int TimesheetID { get; set; }

        [Required]
        public DateTime WeekStartDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal RegularHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal OvertimeHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PublicHolidayHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalHours { get; set; }

        [Required]
        public TimesheetStatus Status { get; set; }

        [Required]
        public int UserID { get; set; }

        public int? SupervisorApprovedByID { get; set; }
        public int? HrApprovedByID { get; set; }
        public int? PayrollProcessedByID { get; set; }
    }
}
