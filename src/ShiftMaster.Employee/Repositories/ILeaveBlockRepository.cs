using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Models;

namespace ShiftMaster.Employee.Repositories
{
    public interface ILeaveBlockRepository
    {
        Task<LeaveBlock?> GetByIdAsync(int id);
        Task<List<LeaveBlock>> GetByUserIdAsync(int userId);
        Task<List<LeaveBlock>> GetPendingByLocationAsync(int locationId);
        Task<bool> UserExistsAsync(int userId);
        Task<bool> LocationExistsAsync(int locationId);
        Task<LeaveBlock> AddAsync(LeaveBlock leave);
        Task SaveAsync();
    }
}
