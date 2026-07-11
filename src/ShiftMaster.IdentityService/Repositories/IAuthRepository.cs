using System.Threading.Tasks;
using ShiftMaster.IdentityService.Models;

namespace ShiftMaster.IdentityService.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetUserByEmailWithRoleAsync(string email);
        Task AddUserAsync(User user);
        Task<int?> GetUserIdByEmailAsync(string email);
        Task<User?> GetUserWithDetailsByIdAsync(int id);
        Task<bool> EmployeeIdExistsAsync(string employeeId);
        Task UpdateUserAsync(User user);
    }
}
