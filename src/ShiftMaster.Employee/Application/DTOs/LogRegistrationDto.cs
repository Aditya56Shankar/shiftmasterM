namespace ShiftMaster.Employee.Application.DTOs
{
    public class LogRegistrationDto
    {
        public int? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string? CorrelationId { get; set; }
        public string Source { get; set; } = "Web";
        public string? Details { get; set; }
        public string? ClientAppVersion { get; set; }
    }
}