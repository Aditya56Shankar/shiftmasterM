using System;
using System.ComponentModel.DataAnnotations;
using ShiftMaster.CommsAuditService.Enums;

namespace ShiftMaster.CommsAuditService.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = null!;

        [Required]
        public NotificationCategory Category { get; set; }

        [Required]
        public NotificationStatus Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
