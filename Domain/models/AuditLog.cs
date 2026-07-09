using System.ComponentModel.DataAnnotations;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class AuditLog
    {
        [Key] public int AuditID { get; set; }
        [Required, MaxLength(255)] public string Action { get; set; }
        [Required, MaxLength(100)] public string EntityType { get; set; }
        public int? RecordID { get; set; } // Nullable to capture failed login attempts
        [Required] public DateTime Timestamp { get; set; }
        public int? UserID { get; set; } // Nullable for unauthenticated events (e.g., failed login)
        public User Actor { get; set; } // The user who performed the action
        public int StatusCode { get; set; }

        // Authentication-specific fields
        public bool IsSuccess { get; set; } = true; // Indicates if the authentication attempt succeeded
        [MaxLength(45)] public string? IPAddress { get; set; } // Client IP address (IPv4/IPv6)
        [MaxLength(512)] public string? UserAgent { get; set; } // Client browser/user agent
        [MaxLength(1000)] public string? Details { get; set; } // Optional freeform details (e.g., failure reason)
        [MaxLength(50)] public string? AuthMethod { get; set; } // Authentication method used (e.g., "Password", "OAuth")
        [MaxLength(64)] public string? CorrelationId { get; set; } // Request correlation ID for tracing
        [MaxLength(50)] public string? Source { get; set; } // Source/channel (e.g., "Web", "Mobile", "API")
        [MaxLength(50)] public string? ClientAppVersion { get; set; } // Optional: client application version
    }
}