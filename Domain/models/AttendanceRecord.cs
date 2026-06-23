using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class AttendanceRecord
    {
        [Key] public int AttendanceID { get; set; }
        [Required] public DateTime WorkDate { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        [Required] public int BreakMinutesTaken { get; set; }
        [Required, Column(TypeName = "decimal(4,2)")] public decimal ActualHoursWorked { get; set; }
        [Required] public int VarianceMinutes { get; set; }
        [Required] public AttendanceStatus Status { get; set; }

        public int AssignmentID { get; set; }
        public ShiftAssignment Assignment { get; set; } =null!;
        public int UserID { get; set; }
        public User Employee { get; set; } = null!;
    }
}
