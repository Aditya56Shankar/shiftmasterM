using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.models;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;

namespace Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDto
                {
                    RoleId = r.roleId,
                    RoleName = r.roleName
                }).ToListAsync();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
        {
            var r = await _context.Roles.FindAsync(roleId);
            if (r == null) return null;

            return new RoleDto
            {
                RoleId = r.roleId,
                RoleName = r.roleName
            };
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto dto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            role.roleName = dto.RoleName;
            await _context.SaveChangesAsync();

            return new RoleDto { RoleId = role.roleId, RoleName = role.roleName };
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            var hasLinkedUsers = await _context.Users.AnyAsync(u => u.RoleID == id);
            if (hasLinkedUsers) throw new InvalidOperationException("Cannot delete role while active profiles hold this access level.");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto newRole)
        {
            var role = new Role
            {
                roleName = newRole.RoleName
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleDto
            {
                RoleId = role.roleId,
                RoleName = role.roleName
            };
        }
    }
}