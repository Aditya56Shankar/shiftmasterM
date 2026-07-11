using System;
using System.Threading.Tasks;

namespace ShiftMaster.AttendanceService.Clients
{
    public interface IEmployeeClient
    {
        Task<bool> HasActiveLeaveOnDateAsync(int userId, DateTime date);
    }
}
