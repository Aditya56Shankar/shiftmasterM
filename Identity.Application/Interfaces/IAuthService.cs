using System;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Application.DTOs;

namespace ShiftMaster.IdentityService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<object> LoginAsync(LoginDto dto);
        Task<int?> GetUserIdByEmailAsync(string email);
        Task<AdminUserDto?> GetAdminUserByIdAsync(int id);
    }
}
