using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.AttendanceService.Models;
using ShiftMaster.AttendanceService.Enums;

namespace ShiftMaster.AttendanceService.Repositories
{
    public interface IOvertimeRepository
    {
        Task<List<OvertimeAuthorisation>> GetPendingOvertimeByUserIdsAsync(List<int> userIds);
        Task<OvertimeAuthorisation> AddOvertimeAsync(OvertimeAuthorisation overtimeAuthorisation);
        Task<OvertimeAuthorisation?> GetOvertimeByIdAsync(int otId);
        Task SaveChangesAsync();
    }
}
