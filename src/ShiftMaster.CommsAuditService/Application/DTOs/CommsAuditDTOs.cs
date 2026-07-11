using System;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.CommsAuditService.Application.DTOs
{
    public class NotificationDto
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateNotificationDto
    {
        [Required]
        public int UserID { get; set; }

        [Required, MaxLength(500)]
        public string Message { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;
    }

    public class AuditLogDto
    {
        public int AuditID { get; set; }
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public int? RecordID { get; set; }
        public DateTime Timestamp { get; set; }
        public int? UserID { get; set; }
        public ActorDto? Actor { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Details { get; set; }
        public string? AuthMethod { get; set; }
        public string? CorrelationId { get; set; }
        public string? Source { get; set; }
        public string? ClientAppVersion { get; set; }
    }

    public class ActorDto
    {
        public int UserID { get; set; }
        public string Name { get; set; } = null!;
    }
}
