using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateUserDto
    {
        [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
        [Required, EmailAddress, MaxLength(150)] public string Email { get; set; } = string.Empty;
        [Required, MaxLength(20)] public string Phone { get; set; } = string.Empty;
        [Required] public string Status { get; set; } = "Active";
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int RoleID { get; set; }
    }
}