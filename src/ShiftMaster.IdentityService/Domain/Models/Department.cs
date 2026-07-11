using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.IdentityService.Domain.Models
{
    public class Department
    {
        [Key] public int departmentId { get; set; }
        [Required] public string departmentName { get; set; } = null!;
        public ICollection<User> Employees { get; set; } = new List<User>();
    }
}
