using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class CreateUserDto
    {
        [Required, MaxLength(50)]
        public string EmployeeID { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // Sent as plain text, we will hash it or placeholder it in the service!

        [Phone, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Active"; // Passed as a readable string ("Active", "Inactive")

        [Required]
        public int LocationID { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        [Required]
        public int RoleID { get; set; }
    }
}