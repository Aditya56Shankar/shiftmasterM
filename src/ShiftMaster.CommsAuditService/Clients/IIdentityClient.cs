using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftMaster.CommsAuditService.Clients
{
    public interface IIdentityClient
    {
        Task<string> GetUserNameAsync(int userId);
        Task<Dictionary<int, string>> LookupUserNamesAsync(List<int> userIds);
    }
}
