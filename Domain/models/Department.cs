using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Interfaces;
using ShiftMaster.models;

namespace Domain.models
{
    public class Department : IMustHaveTenant
    {
        [Key] public int departmentId { get; set; }
        [Required] public string departmentName { get; set; }
        public ICollection<User> Employees { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
