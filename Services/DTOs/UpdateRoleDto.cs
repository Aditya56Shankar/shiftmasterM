using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateRoleDto
    {
        [Required, MaxLength(50)] public string RoleName { get; set; }
    }
}