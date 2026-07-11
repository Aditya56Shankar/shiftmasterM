using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.SchedulingService.Enums;

namespace ShiftMaster.SchedulingService.Models
{
    public class ShiftPattern
    {
        [Key] public int PatternID { get; set; }
        [Required, MaxLength(100)] public string PatternName { get; set; } = null!;
        [Required] public TimeSpan StartTime { get; set; }
        [Required] public TimeSpan EndTime { get; set; }
        [Required, Column(TypeName = "decimal(4,2)")] public decimal DurationHours { get; set; }
        [Required] public int BreakMinutes { get; set; }
        [Required] public ShiftType ShiftType { get; set; }
        [Required] public int MinStaffingLevel { get; set; }
        [Required] public ActiveStatus Status { get; set; }

        [Required] public int LocationID { get; set; } // Foreign Key to Identity Service WorkLocation

        public ICollection<ShiftAssignment> Assignments { get; set; } = new List<ShiftAssignment>();
    }
}
