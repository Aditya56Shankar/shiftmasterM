namespace ShiftMaster.CommsAuditService.Domain.Enums
{
    public enum NotificationCategory
    {
        Roster,
        Shift,
        Swap,
        Cover,
        Overtime,
        Attendance,
        Compliance
    }

    public enum NotificationStatus
    {
        Unread,
        Read,
        Dismissed
    }
}
