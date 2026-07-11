using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShiftMaster.SchedulingService.Domain.Enums;

namespace ShiftMaster.SchedulingService.Domain.Models
{
    public class WeeklyRoster
    {
        [Key] public int RosterID { get; set; }
        [Required] public DateTime WeekStartDate { get; set; }
        [Required] public DateTime WeekEndDate { get; set; }
        public DateTime PublishedDate { get; set; }
        [Required] public RosterStatus Status { get; set; }

        // Foreign Keys & Navigation properties removed for microservice boundaries
        [Required] public int LocationID { get; set; }
        [Required] public int CreatedByID { get; set; }
        [Required] public int DepartmentID { get; set; }
        public int? ApprovedByUserID { get; set; }

        public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
        public ICollection<SchedulingConstraintViolation> Violations { get; set; } = new List<SchedulingConstraintViolation>();
    }
}
