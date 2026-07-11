using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.SchedulingService.Enums
{
    public enum ActiveStatus
    {
        Active,
        Inactive
    }

    public enum RosterStatus
    {
        Draft,
        PendingApproval,
        Published,
        Amended
    }

    public enum ShiftAssignmentStatus
    {
        Assigned,
        Cancelled,
        Confirmed,
        Swapped,
        Covered
    }

    public enum SeverityLevel
    {
        Warning,
        Blocking
    }

    public enum ViolationStatus
    {
        Open,
        Overridden,
        Resolved
    }

    public enum ShiftType
    {
        Morning,
        Afternoon,
        Night,
        Split,
        Rotating
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed
    }

    public enum CoverType
    {
        Voluntary,
        Mandatory,
        EmergencyCalling
    }

    public enum CoverStatus
    {
        Assigned,
        Confirmed,
        Completed
    }

    public enum ViolationType
    {
        [Display(Name = "Insufficient Rest Between Shifts")]
        [Description("Employee does not have the mandatory 11-hour rest window between scheduled work blocks.")]
        InsufficientRest = 1,

        [Display(Name = "Maximum Weekly Hours Exceeded")]
        [Description("Employee's total combined shift lengths exceed the allowable 40-hour weekly threshold.")]
        MaxHoursExceeded = 2,

        [Display(Name = "Employee Skill Gap")]
        [Description("Employee lacks a specific mandatory credential or training index required for this layout.")]
        SkillGap = 3,

        [Display(Name = "Employee Unavailable")]
        [Description("Employee has an active approved leave block or personal unavailability submission for this date.")]
        UnavailableEmployee = 4,

        [Display(Name = "Invalid Skill Coverage")]
        [Description("The collective shift assignment lacks the required structural skill matrix required for safety compliance.")]
        InvalidSkillCoverage = 5,

        [Display(Name = "Invalid staff Coverage")]
        [Description("The collective shift assignment lacks the required structural staff .")]
        UnderStaffed = 6,

        [Display(Name = "Invalid date assigned")]
        [Description("The collective shift assignment date is past.")]
        InvalidRosterDate = 7
    }
}
