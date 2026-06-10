using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.models
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }
        [Required, MaxLength(150)]
        public string CompanyName { get; set; }
        [MaxLength(100)]
        public string Domain { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
