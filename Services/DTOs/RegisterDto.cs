using System;
using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string EmployeeID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; }

        // Inside Services.DTOs.RegisterDto

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Phone, MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        public int LocationID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int DepartmentID { get; set; }
    }
}