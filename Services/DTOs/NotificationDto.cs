using System;

namespace Services.DTOs
{
    public class NotificationDto
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; } 
        public string Message { get; set; }
        public string Category { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}