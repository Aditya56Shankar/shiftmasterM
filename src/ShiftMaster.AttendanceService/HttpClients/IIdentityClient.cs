using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftMaster.AttendanceService.Clients
{
    public interface IIdentityClient
    {
        Task<bool> UserExistsAsync(int userId);
        Task<bool> WorkLocationExistsAsync(int locationId);
        Task<List<int>> GetUserIdsByLocationAsync(int locationId);
        Task<System.Collections.Generic.Dictionary<int, string>> LookupUserNamesAsync(System.Collections.Generic.List<int> userIds);
    }
}
