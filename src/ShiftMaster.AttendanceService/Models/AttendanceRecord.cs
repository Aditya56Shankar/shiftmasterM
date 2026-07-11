using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.AttendanceService.Enums;

namespace ShiftMaster.AttendanceService.Models
{
    public class AttendanceRecord
    {
        [Key]
        public int AttendanceID { get; set; }

        [Required]
        public DateTime WorkDate { get; set; }

        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }

        [Required]
        public int BreakMinutesTaken { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal ActualHoursWorked { get; set; }

        [Required]
        public int VarianceMinutes { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; }

        [Required]
        public int AssignmentID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int LocationID { get; set; }
    }
}
