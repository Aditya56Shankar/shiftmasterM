using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateDepartmentDto
    {
        [Required] public string DepartmentName { get; set; } = string.Empty;
    }
}