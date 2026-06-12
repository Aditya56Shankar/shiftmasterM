using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ShiftMaster.models;

namespace Domain.models
{
    public class Role
    {
        [Key] public int roleId { get; set; }
        [Required, MaxLength(50)] public string roleName { get; set; }

        // Navigation Property: One Role can have many Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
