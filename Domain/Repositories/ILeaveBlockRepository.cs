using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Domain.Repositories
{
    public interface ILeaveBlockRepository
    {
        Task<LeaveBlock> AddAsync(LeaveBlock leave);
        Task<LeaveBlock?> GetByIdAsync(int id);
        Task<List<LeaveBlock>> GetByUserIdAsync(int userId);
        Task<List<LeaveBlock>> GetPendingByLocationAsync(int locationId);
        Task<bool> UserExistsAsync(int userId);
        Task<bool> LocationExistsAsync(int locationId);
        Task SaveAsync();
    }
}
