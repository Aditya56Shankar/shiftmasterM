using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftMaster.SchedulingService.Clients
{
    public class UserShortDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public interface IIdentityClient
    {
        Task<bool> UserExistsAsync(int userId);
        Task<bool> LocationExistsAsync(int locationId);
        Task<bool> DepartmentExistsAsync(int departmentId);
        Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds);
        Task<string?> GetLocationNameAsync(int locationId);
        Task<string?> GetDepartmentNameAsync(int departmentId);
        Task<List<int>> GetUserIdsByLocationAsync(int locationId);
        Task<List<UserShortDto>> GetActiveUsersByLocationAndDeptAsync(int locationId, int departmentId);
        Task<List<UserShortDto>> GetActiveUsersByLocationAsync(int locationId);
    }
}
