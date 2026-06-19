using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Services.DTOs
{
    public class EmployeeSkillRequestDto
    {
        public int UserID { get; set; }
        public string SkillName { get; set; }
        public ProficiencyLevel ProficiencyLevel { get; set; }
        public DateTime CertifiedDate { get; set; }
    }
}
