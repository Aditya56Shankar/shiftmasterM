using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateUserDto
    {
        [Required, MaxLength(100)] public string Name { get; set; } 
        [Required, EmailAddress, MaxLength(150)] public string Email { get; set; }
        [Required, MaxLength(20)] public string Phone { get; set; } 
        [Required] public string Status { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int RoleID { get; set; }
    }
}