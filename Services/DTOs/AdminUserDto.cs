using System;
using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class AdminUserDto
    {
        public int UserId { get; set; }

        public string EmployeeID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string LocationName { get; set; }

        public string RoleName { get; set; }

        public string DepartmentName { get; set; }
    }
}
