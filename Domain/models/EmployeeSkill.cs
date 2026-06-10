using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class EmployeeSkill : IMustHaveTenant
    {
        [Key] public int EmpSkillID { get; set; }
        [Required, MaxLength(100)] public string SkillName { get; set; }
        [Required] public ProficiencyLevel ProficiencyLevel { get; set; }
        [Required] public DateTime CertifiedDate { get; set; }
        [Required] public ActiveStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int UserID { get; set; }
        public User Employee { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
