using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Application.DTOs;

namespace ShiftMaster.Employee.Clients
{
    public interface ISchedulingClient
    {
        Task<List<EmployeeShiftDto>> GetShiftsByUserIdAsync(int userId);
    }
}
