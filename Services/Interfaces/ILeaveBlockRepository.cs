using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces
{
    public interface ILeaveBlockRepository
    {
        Task<LeaveBlock> AddLeaveBlockAsync(LeaveBlock leave);
    }
}
