using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.models;
using shiftmaster.models;

namespace ShiftMaster.models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required, MaxLength(50)] public string EmployeeID { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; }
        [Required, EmailAddress, MaxLength(150)] public string Email { get; set; }
        [Required] public string PasswordHash { get; set; }
        [Phone, MaxLength(20)] public string Phone { get; set; }
        [Required] public UserStatus Status { get; set; }

        // Foreign Keys
        [Required] public int LocationID { get; set; }
        [Required] public int RoleID { get; set; }
        [Required] public int DepartmentID { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        // Navigation Properties
        public Department Department { get; set; }
        public WorkLocation HomeLocation { get; set; }
        public Role Role { get; set; }
        public ICollection<ShiftAssignment> Shifts { get; set; } = new List<ShiftAssignment>();
        public ICollection<LeaveBlock> LeaveBlocks { get; set; } = new List<LeaveBlock>();
        public ICollection<AvailabilitySubmission> Availabilities { get; set; } = new List<AvailabilitySubmission>();
        public ICollection<EmployeeSkill> Skills { get; set; } = new List<EmployeeSkill>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}