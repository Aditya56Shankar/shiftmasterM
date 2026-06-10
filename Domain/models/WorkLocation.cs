using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class WorkLocation : IMustHaveTenant
    {
        [Key] public int LocationID { get; set; }
        [Required, MaxLength(100)] public string LocationName { get; set; }
        [Required] public LocationType Type { get; set; }
        [Required, MaxLength(100)] public string City { get; set; }
        [MaxLength(100)] public string OperatingHours { get; set; }
        [Required] public ActiveStatus Status { get; set; }

        // Foreign Keys & Navigation
        public int? ManagerID { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public User Manager { get; set; }
        public ICollection<User> Employees { get; set; } = new List<User>();
        public ICollection<ShiftPattern> ShiftPatterns { get; set; } = new List<ShiftPattern>();
        public ICollection<SkillRequirement> SkillRequirements { get; set; } = new List<SkillRequirement>();
        public ICollection<WeeklyRoster> Rosters { get; set; } = new List<WeeklyRoster>();
    }
}
