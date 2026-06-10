using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class LabourReport : IMustHaveTenant
    {
        [Key] public int ReportID { get; set; }
        [Required] public ReportScope Scope { get; set; }
        [Required] public string Metrics { get; set; } // JSON Payload
        [Required] public DateTime GeneratedDate { get; set; }

        // Optional tracking of who requested the report
        public int? GeneratedByID { get; set; }
        public User GeneratedBy { get; set; }

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
