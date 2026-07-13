namespace ShiftMaster.AttendanceService.Domain.Enums
{
    public enum AttendanceStatus
    {
        Present,
        Late,
        EarlyLeave,
        Absent,
        Excused
    }

    public enum TimesheetStatus
    {
        Draft,
        Submitted,
        SupervisorApproved,
        HrApproved,
        SentToPayroll
    }

    public enum OTType
    {
        Scheduled,
        Emergency,
        StayBack
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed
    }

    public enum ReportScope
    {
        Location,
        Department,
        ShiftType,
        Period
    }
}
