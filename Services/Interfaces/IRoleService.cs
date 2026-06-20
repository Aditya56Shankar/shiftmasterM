using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;

namespace Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int roleId);
        Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto dto);
        Task<bool> DeleteRoleAsync(int id);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto newRole);
    }
}