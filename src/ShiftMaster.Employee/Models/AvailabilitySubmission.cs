using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.Employee.Enums;

namespace ShiftMaster.Employee.Models
{
    public class AvailabilitySubmission
    {
        [Key] public int AvailabilityID { get; set; }
        [Required] public DateTime WeekStartDate { get; set; }
        [Required, MaxLength(200)] public string AvailableDays { get; set; } = null!;
        [MaxLength(50)] public string? PreferredShiftType { get; set; }
        [Required, Column(TypeName = "decimal(5,2)")] public decimal MaxHoursPerWeek { get; set; }
        [Required] public DateTime SubmittedDate { get; set; }
        [Required] public AvailabilityStatus Status { get; set; }

        [Required] public int UserID { get; set; }
    }
}
