using System;
using System.ComponentModel.DataAnnotations;
using ShiftMaster.Employee.Enums;

namespace ShiftMaster.Employee.Models
{
    public class LeaveBlock
    {
        [Key] public int LeaveID { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public LeaveReason Reason { get; set; }
        [Required] public LeaveStatus Status { get; set; }

        // Foreign Keys & Navigation removed for microservice boundary
        [Required] public int UserID { get; set; }
        public int? ApprovedByID { get; set; }
    }
}
