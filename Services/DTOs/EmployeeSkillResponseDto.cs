using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class EmployeeSkillResponseDto
    {
        public int EmpSkillID { get; set; }
        public int UserID { get; set; }
        public string SkillName { get; set; }
        public string ProficiencyLevel { get; set; }
        public DateTime CertifiedDate { get; set; }
        public string Status { get; set; }
    }
}
