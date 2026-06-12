using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class LabourReport
    {
        [Key] public int ReportID { get; set; }
        [Required] public ReportScope Scope { get; set; }
        [Required] public string Metrics { get; set; } // JSON Payload
        [Required] public DateTime GeneratedDate { get; set; }

        // Optional tracking of who requested the report
        public int? GeneratedByID { get; set; }
        public User GeneratedBy { get; set; }
    }
}
