using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateSkillRequirementDto
    {
        [Required, MaxLength(100)] public string SkillName { get; set; } = string.Empty;
        [Required] public int MinCountPerShift { get; set; }
        [Required] public string Status { get; set; } = "Active";
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
    }
}