using System;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.IdentityService.Application.DTOs
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string EmployeeID { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;

        [Phone, MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        public int LocationID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int DepartmentID { get; set; }
    }

    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

    public class AdminUserDto
    {
        public int UserId { get; set; }
        public string EmployeeID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? LocationName { get; set; }
        public string? RoleName { get; set; }
        public string? DepartmentName { get; set; }
    }

    public class UserDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Status { get; set; } = null!;
        public int LocationID { get; set; }
        public string LocationName { get; set; } = null!;
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int RoleID { get; set; }
    }

    public class CreateUserDto
    {
        [Required, MaxLength(50)]
        public string EmployeeID { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Phone, MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public int LocationID { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        [Required]
        public int RoleID { get; set; }
    }

    public class UpdateUserDto
    {
        [Required, MaxLength(100)] public string Name { get; set; } = null!;
        [Required, EmailAddress, MaxLength(150)] public string Email { get; set; } = null!;
        [Required, MaxLength(20)] public string Phone { get; set; } = null!;
        [Required] public string Status { get; set; } = null!;
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int RoleID { get; set; }
    }

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }

    public class CreateRoleDto
    {
        [Required, MaxLength(50)]
        public string RoleName { get; set; } = null!;
    }

    public class UpdateRoleDto
    {
        [Required, MaxLength(50)]
        public string RoleName { get; set; } = null!;
    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
    }

    public class CreateDepartmentDto
    {
        [Required]
        public string DepartmentName { get; set; } = null!;
    }

    public class UpdateDepartmentDto
    {
        [Required]
        public string DepartmentName { get; set; } = null!;
    }

    public class WorkLocationDto
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string City { get; set; } = null!;
        public string OperatingHours { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? ManagerID { get; set; }
    }

    public class CreateWorkLocationDto
    {
        [Required, MaxLength(100)] public string LocationName { get; set; } = null!;
        [Required] public string Type { get; set; } = null!;
        [Required, MaxLength(100)] public string City { get; set; } = null!;
        [Required, MaxLength(100)] public string OperatingHours { get; set; } = null!;
        [Required] public string Status { get; set; } = null!;
        public int? ManagerID { get; set; }
    }

    public class UpdateWorkLocationDto
    {
        [Required, MaxLength(100)] public string LocationName { get; set; } = null!;
        [Required] public string Type { get; set; } = null!;
        [Required, MaxLength(100)] public string City { get; set; } = null!;
        [Required, MaxLength(100)] public string OperatingHours { get; set; } = null!;
        [Required] public string Status { get; set; } = null!;
        public int? ManagerID { get; set; }
    }

    public class LogLoginDto
    {
        public int? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string AuthMethod { get; set; } = "Password";
        public string? CorrelationId { get; set; }
        public string Source { get; set; } = "Web";
        public string? Details { get; set; }
        public string? ClientAppVersion { get; set; }
    }

    public class LogRegistrationDto
    {
        public int? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string? CorrelationId { get; set; }
        public string Source { get; set; } = "Web";
        public string? Details { get; set; }
        public string? ClientAppVersion { get; set; }
    }

    public class LogEventDto
    {
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public int? RecordId { get; set; }
        public int? UserId { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string? Details { get; set; }
    }
}
