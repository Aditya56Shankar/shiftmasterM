using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class UpdateSkillRequirementDto
    {
        [Required, MaxLength(100)] public string SkillName { get; set; }
        [Required] public int MinCountPerShift { get; set; }
        [Required] public string Status { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
    }
}