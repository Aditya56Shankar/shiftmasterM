using System;
using System.ComponentModel.DataAnnotations;
using ShiftMaster.AttendanceService.Domain.Enums;

namespace ShiftMaster.AttendanceService.Domain.Models
{
    public class LabourReport
    {
        [Key]
        public int ReportID { get; set; }

        [Required]
        public ReportScope Scope { get; set; }

        [Required]
        public string Metrics { get; set; } = null!; // JSON payload

        [Required]
        public DateTime GeneratedDate { get; set; }

        public int? GeneratedByID { get; set; }
    }
}
