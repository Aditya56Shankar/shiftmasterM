using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class TimesheetSummary : IMustHaveTenant
    {
        [Key] public int TimesheetID { get; set; }
        [Required] public DateTime WeekStartDate { get; set; }
        [Required, Column(TypeName = "decimal(5,2)")] public decimal RegularHours { get; set; }
        [Required, Column(TypeName = "decimal(5,2)")] public decimal OvertimeHours { get; set; }
        [Required, Column(TypeName = "decimal(5,2)")] public decimal PublicHolidayHours { get; set; }
        [Required, Column(TypeName = "decimal(5,2)")] public decimal TotalHours { get; set; }
        [Required] public TimesheetStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int UserID { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public User Employee { get; set; }

        public int? ApprovedByID { get; set; }
        public User ApprovedBy { get; set; }
    }
}
