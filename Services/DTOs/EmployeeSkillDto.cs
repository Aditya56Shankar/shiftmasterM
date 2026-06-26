using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Services.DTOs
{
    public class EmployeeSkillDto
    {
        public int EmpSkillID { get; set; }
        public string SkillName { get; set; }
        public string ProficiencyLevel { get; set; }
        public DateTime CertifiedDate { get; set; }
        public string Status { get; set; }
    }
}
