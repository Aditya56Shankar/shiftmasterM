using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class Notification : IMustHaveTenant
    {
        [Key] public int NotificationID { get; set; }
        [Required, MaxLength(500)] public string Message { get; set; }
        [Required] public NotificationCategory Category { get; set; }
        [Required] public NotificationStatus Status { get; set; }
        [Required] public DateTime CreatedDate { get; set; }

        // Foreign Keys & Navigation
        [Required] public int UserID { get; set; }
        public User Employee { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
