using System;
using System.ComponentModel.DataAnnotations;
using ShiftMaster.Employee.Enums;

namespace ShiftMaster.Employee.Models
{
    public class EmployeeSkill
    {
        [Key] public int EmpSkillID { get; set; }
        [Required, MaxLength(100)] public string SkillName { get; set; } = null!;
        [Required] public ProficiencyLevel ProficiencyLevel { get; set; }
        [Required] public DateTime CertifiedDate { get; set; }
        [Required] public ActiveStatus Status { get; set; }

        [Required] public int UserID { get; set; }
    }
}
