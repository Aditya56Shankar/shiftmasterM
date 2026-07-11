using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftMaster.Employee.Clients
{
    public interface IIdentityClient
    {
        Task<bool> UserExistsAsync(int userId);
        Task<bool> LocationExistsAsync(int locationId);
        Task<List<int>> GetUserIdsByLocationAsync(int locationId);
        Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds);
        Task<string?> GetLocationNameAsync(int locationId);
        Task<string?> GetDepartmentNameAsync(int departmentId);
    }
}
