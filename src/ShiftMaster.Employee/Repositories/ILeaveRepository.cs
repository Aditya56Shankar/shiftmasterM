using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Models;

namespace ShiftMaster.Employee.Repositories
{
    public interface ILeaveRepository
    {
        Task<List<LeaveBlock>> GetActiveLeavesAsync(int userId);
    }
}
