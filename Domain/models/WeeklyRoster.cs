using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class WeeklyRoster
    {
        [Key] public int RosterID { get; set; }
        [Required] public DateTime WeekStartDate { get; set; }
        [Required] public DateTime WeekEndDate { get; set; }
        public DateTime PublishedDate { get; set; }
        [Required] public RosterStatus Status { get; set; }

        // Foreign Keys & Navigation
        [Required] 
        public int LocationID { get; set; }
        public WorkLocation Location { get; set; }

        //[Required] 
        public int? CreatedByID { get; set; }
        public User CreatedBy { get; set; }

        [Required] 
        public int DepartmentID { get; set; }
        public Department Department { get; set; }


        public int? ApprovedByUserID { get; set; }

        public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
        public ICollection<SchedulingConstraintViolation> Violations { get; set; } = new List<SchedulingConstraintViolation>();
    }
}
