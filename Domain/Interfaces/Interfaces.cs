using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IMustHaveTenant
    {
        int TenantId { get; set; }
    }
}
