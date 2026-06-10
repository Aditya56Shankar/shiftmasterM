using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class AvailabilitySubmission : IMustHaveTenant
    {
        [Key] public int AvailabilityID { get; set; }
        [Required] public DateTime WeekStartDate { get; set; }
        [Required, MaxLength(200)] public string AvailableDays { get; set; }
        [MaxLength(50)] public string PreferredShiftType { get; set; }
        [Required, Column(TypeName = "decimal(5,2)")] public decimal MaxHoursPerWeek { get; set; }
        [Required] public DateTime SubmittedDate { get; set; }
        [Required] public AvailabilityStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int UserID { get; set; }
        public User Employee { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
