using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateWorkLocationDto
    {
        [Required, MaxLength(100)] public string LocationName { get; set; } = string.Empty;
        [Required, MaxLength(50)] public string Type { get; set; } = string.Empty;
        [Required, MaxLength(100)] public string City { get; set; } = string.Empty;
        [Required, MaxLength(100)] public string OperatingHours { get; set; } = string.Empty;
        [Required] public string Status { get; set; } = "Active";
        public int ManagerID { get; set; }
    }
}