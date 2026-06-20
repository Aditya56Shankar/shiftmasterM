using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class AuditLogDto
    {
        public int AuditID { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
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
}
