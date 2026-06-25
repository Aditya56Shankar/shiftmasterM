using System.Threading.Tasks;
using Services.DTOs;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<int?> GetUserIdByEmailAsync(string email);
        Task<AdminUserDto> GetAdminUserByIdAsync(int id);
    }
}