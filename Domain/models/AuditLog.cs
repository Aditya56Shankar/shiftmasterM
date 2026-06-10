using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;
using Domain.models;
using ShiftMaster.models;

namespace shiftmaster.models
{
    public class AuditLog : IMustHaveTenant
    {
        [Key] public int AuditID { get; set; }
        [Required, MaxLength(255)] public string Action { get; set; }
        [Required, MaxLength(100)] public string EntityType { get; set; }
        [Required] public int RecordID { get; set; }
        [Required] public DateTime Timestamp { get; set; }
        [Required] public int UserID { get; set; }
        public User Actor { get; set; } // The user who performed the action

        [Required]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
