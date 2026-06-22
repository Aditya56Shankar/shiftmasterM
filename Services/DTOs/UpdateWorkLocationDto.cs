using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateWorkLocationDto
    {
        [Required, MaxLength(100)] public string LocationName { get; set; } 
        [Required, MaxLength(50)] public string Type { get; set; } 
        [Required, MaxLength(100)] public string City { get; set; }
        [Required, MaxLength(100)] public string OperatingHours { get; set; } 
        [Required] public string Status { get; set; } 
        public int ManagerID { get; set; }
    }
}