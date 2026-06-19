using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class CreateRoleDto
    {
        [Required, MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;
    }
}