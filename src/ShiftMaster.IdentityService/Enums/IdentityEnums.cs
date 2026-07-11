namespace ShiftMaster.IdentityService.Enums
{
    public enum ActiveStatus
    {
        Active,
        Inactive
    }

    public enum LocationType
    {
        Store,
        Plant,
        ContactCentre,
        Ward,
        Hotel
    }

    public enum UserStatus
    {
        Active,
        OnLeave,
        Inactive,
        Terminated
    }

    public enum UserRole
    {
        Employee,
        Supervisor,
        HR,
        OpsManager,
        Payroll,
        SchedulingAdmin
    }
}
