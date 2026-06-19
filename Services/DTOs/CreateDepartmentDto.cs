using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        public string DepartmentName { get; set; } = string.Empty;
    }
}
