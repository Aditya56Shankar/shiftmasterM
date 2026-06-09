using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using shiftmaster.models;

namespace ShiftMaster.models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required, MaxLength(50)] public string EmployeeID { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; }
        [Required] public UserRole Role { get; set; }
        [Required, EmailAddress, MaxLength(150)] public string Email { get; set; }
        [Required] public string PasswordHash { get; set; }
        [Phone, MaxLength(20)] public string Phone { get; set; }
        [Required] public UserStatus Status { get; set; }

        // Foreign Keys
        [Required] public int LocationID { get; set; }

        // Navigation Properties
        public WorkLocation HomeLocation { get; set; }
        public ICollection<ShiftAssignment> Shifts { get; set; } = new List<ShiftAssignment>();
        public ICollection<LeaveBlock> LeaveBlocks { get; set; } = new List<LeaveBlock>();
        public ICollection<AvailabilitySubmission> Availabilities { get; set; } = new List<AvailabilitySubmission>();
        public ICollection<EmployeeSkill> Skills { get; set; } = new List<EmployeeSkill>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}