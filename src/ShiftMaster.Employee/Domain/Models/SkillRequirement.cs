using System.ComponentModel.DataAnnotations;
using ShiftMaster.Employee.Domain.Enums;

namespace ShiftMaster.Employee.Domain.Models
{
    public class SkillRequirement
    {
        [Key] public int SkillReqID { get; set; }
        [Required, MaxLength(100)] public string SkillName { get; set; } = null!;
        [Required] public int MinCountPerShift { get; set; }
        [Required] public ActiveStatus Status { get; set; }

        // Foreign Keys & Navigation removed for microservice boundary
        [Required] public int LocationID { get; set; }
        [Required] public int DepartmentID { get; set; }
    }
}
