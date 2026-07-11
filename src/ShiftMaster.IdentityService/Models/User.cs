using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShiftMaster.IdentityService.Enums;

namespace ShiftMaster.IdentityService.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required, MaxLength(50)] public string EmployeeID { get; set; } = null!;
        [Required, MaxLength(100)] public string Name { get; set; } = null!;
        [Required, EmailAddress, MaxLength(150)] public string Email { get; set; } = null!;
        [Required] public string PasswordHash { get; set; } = null!;
        [Phone, MaxLength(20)] public string? Phone { get; set; }
        [Required] public UserStatus Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Foreign Keys
        [Required] public int LocationID { get; set; }
        [Required] public int RoleID { get; set; }
        [Required] public int DepartmentID { get; set; }

        // Navigation Properties
        public Department Department { get; set; } = null!;
        public WorkLocation HomeLocation { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
