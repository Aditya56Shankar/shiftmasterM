using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.IdentityService.Domain.Models
{
    public class Role
    {
        [Key] public int roleId { get; set; }
        [Required, MaxLength(50)] public string roleName { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
