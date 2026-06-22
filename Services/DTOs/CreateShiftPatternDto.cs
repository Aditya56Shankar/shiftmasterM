using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class CreateShiftPatternDto
    {
        [Required, MaxLength(100)] public string PatternName { get; set; }
        [Required] public string StartTime { get; set; }  // Standard "HH:mm:ss"
        [Required] public string EndTime { get; set; }  // Standard "HH:mm:ss"
        [Required] public decimal DurationHours { get; set; }
        [Required] public int BreakMinutes { get; set; }
        [Required] public string ShiftType { get; set; }     // Maps to ShiftType enum
        [Required] public int MinStaffingLevel { get; set; }
        [Required] public string Status { get; set; }           // Maps to ActiveStatus enum
        [Required] public int LocationID { get; set; }
    }
}