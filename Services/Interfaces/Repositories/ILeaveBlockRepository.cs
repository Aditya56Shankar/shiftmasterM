using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces.Repositories
{
    public interface ILeaveBlockRepository
    {
        Task<LeaveBlock> AddAsync(LeaveBlock leave);
        Task<LeaveBlock?> GetByIdAsync(int id);
        Task SaveAsync();
    }
}
