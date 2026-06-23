using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using shiftmaster.models;

namespace Services.Interfaces
{
    public interface ILeaveBlockService
    {
        Task<LeaveBlock> AddLeaveBlockAsync(LeaveBlock leave);

        Task<bool> UpdateLeaveStatusAsync(int id, LeaveStatus status, int approvedBy); 

    }
}
