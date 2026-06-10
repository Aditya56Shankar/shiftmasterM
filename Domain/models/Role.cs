using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Interfaces;
using ShiftMaster.models;

namespace Domain.models
{
    public class Role : IMustHaveTenant
    {
        [Key] public int roleId { get; set; }
        [Required, MaxLength(50)] public string roleName { get; set; }

        //Foreign Keys
        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        // Navigation Property: One Role can have many Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
