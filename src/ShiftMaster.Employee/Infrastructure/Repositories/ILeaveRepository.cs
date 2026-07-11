using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Domain.Models;

namespace ShiftMaster.Employee.Infrastructure.Repositories
{
    public interface ILeaveRepository
    {
        Task<List<LeaveBlock>> GetActiveLeavesAsync(int userId);
    }
}
