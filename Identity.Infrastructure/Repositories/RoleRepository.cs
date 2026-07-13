using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Domain.Models;
using ShiftMaster.IdentityService.Infrastructure.Data;

namespace ShiftMaster.IdentityService.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdentityDbContext _context;

        public RoleRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync() =>
            await _context.Roles.ToListAsync();

        public async Task<Role?> GetByIdAsync(int id) =>
            await _context.Roles.FindAsync(id);

        public async Task AddAsync(Role role) =>
            await _context.Roles.AddAsync(role);

        public void Remove(Role role) =>
            _context.Roles.Remove(role);

        public async Task<bool> HasLinkedUsersAsync(int roleId) =>
            await _context.Users.AnyAsync(u => u.RoleID == roleId);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
