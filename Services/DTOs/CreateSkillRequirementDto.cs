using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class CreateSkillRequirementDto
    {
        [Required, MaxLength(100)]
        public string SkillName { get; set; }

        [Required]
        public int MinCountPerShift { get; set; }

        [Required]
        public string Status { get; set; } // Passed as "Active" or "Inactive" string

        [Required]
        public int LocationID { get; set; }

        [Required]
        public int DepartmentID { get; set; }
    }
}