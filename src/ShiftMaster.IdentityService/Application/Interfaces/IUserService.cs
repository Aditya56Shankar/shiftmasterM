using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Application.DTOs;

namespace ShiftMaster.IdentityService.Application.Interfaces
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
