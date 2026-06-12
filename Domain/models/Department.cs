using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ShiftMaster.models;

namespace Domain.models
{
    public class Department
    {
        [Key] public int departmentId { get; set; }
        [Required] public string departmentName { get; set; }
        public ICollection<User> Employees { get; set; }
    }
}
