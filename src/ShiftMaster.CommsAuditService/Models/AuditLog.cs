using System;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.CommsAuditService.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Action { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string EntityType { get; set; } = null!;

        public int? RecordID { get; set; } // Nullable to capture failed login attempts

        [Required]
        public DateTime Timestamp { get; set; }

        public int? UserID { get; set; } // Nullable for unauthenticated events (e.g., failed login)

        public int StatusCode { get; set; }

        // Authentication-specific fields
        public bool IsSuccess { get; set; } = true;

        [MaxLength(45)]
        public string? IPAddress { get; set; }

        [MaxLength(512)]
        public string? UserAgent { get; set; }

        [MaxLength(1000)]
        public string? Details { get; set; }

        [MaxLength(50)]
        public string? AuthMethod { get; set; }

        [MaxLength(64)]
        public string? CorrelationId { get; set; }

        [MaxLength(50)]
        public string? Source { get; set; }

        [MaxLength(50)]
        public string? ClientAppVersion { get; set; }
    }
}
