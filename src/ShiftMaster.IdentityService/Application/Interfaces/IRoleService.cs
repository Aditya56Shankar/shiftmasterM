using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Application.DTOs;

namespace ShiftMaster.IdentityService.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int roleId);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto newRole);
        Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto dto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
