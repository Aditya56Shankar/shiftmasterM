using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace shiftmaster.models
{
    public class ShiftPattern
    {
        [Key] public int PatternID { get; set; }
        [Required, MaxLength(100)] public string PatternName { get; set; }
        [Required] public TimeSpan StartTime { get; set; }
        [Required] public TimeSpan EndTime { get; set; }
        [Required, Column(TypeName = "decimal(4,2)")] public decimal DurationHours { get; set; }
        [Required] public int BreakMinutes { get; set; }
        [Required] public ShiftType ShiftType { get; set; }
        [Required] public int MinStaffingLevel { get; set; }
        [Required] public ActiveStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int LocationID { get; set; }
        public WorkLocation Location { get; set; }

        public ICollection<ShiftAssignment> Assignments { get; set; } = new List<ShiftAssignment>();
    }
}
