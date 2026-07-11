using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShiftMaster.IdentityService.Enums;

namespace ShiftMaster.IdentityService.Models
{
    public class WorkLocation
    {
        [Key] public int LocationID { get; set; }
        [Required, MaxLength(100)] public string LocationName { get; set; } = null!;
        [Required] public LocationType Type { get; set; }
        [Required, MaxLength(100)] public string City { get; set; } = null!;
        [MaxLength(100)] public string OperatingHours { get; set; } = null!;
        [Required] public ActiveStatus Status { get; set; }

        // Foreign Keys & Navigation
        public int? ManagerID { get; set; }
        public User? Manager { get; set; }
        public ICollection<User> Employees { get; set; } = new List<User>();
    }
}
