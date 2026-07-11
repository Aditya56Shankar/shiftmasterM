using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.DTOs;

namespace ShiftMaster.IdentityService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto newUser);
    }
}
