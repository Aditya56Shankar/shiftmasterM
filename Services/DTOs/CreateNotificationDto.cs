using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class CreateNotificationDto
    {
        [Required]
        public int UserID { get; set; }

        [Required, MaxLength(500)]
        public string Message { get; set; }

        [Required]
        public string Category { get; set; }  // E.g., Roster, Shift, Swap, Cover [cite: 228]
    }
}