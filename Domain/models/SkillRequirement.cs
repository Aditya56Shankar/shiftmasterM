using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace shiftmaster.models
{
    public class SkillRequirement
    {
        [Key] public int SkillReqID { get; set; }
        [Required, MaxLength(100)] public string SkillName { get; set; }
        [Required] public int MinCountPerShift { get; set; }
        [Required] public ActiveStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] public int LocationID { get; set; }
        public WorkLocation Location { get; set; }
    }
}
