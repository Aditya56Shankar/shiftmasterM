namespace ShiftMaster.Employee.Enums
{
    public enum ActiveStatus
    {
        Active,
        Inactive
    }

    public enum LeaveReason
    {
        ApprovedLeave,
        PublicHoliday,
        Medical,
        Restricted
    }

    public enum LeaveStatus
    {
        Pending,
        Active,
        Cancelled
    }

    public enum ProficiencyLevel
    {
        Trainee,
        Proficient,
        Expert
    }

    public enum AvailabilityStatus
    {
        Submitted,
        Acknowledged,
        Overridden
    }
}
