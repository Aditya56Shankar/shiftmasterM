using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Data;
using ShiftMaster.IdentityService.Models;

namespace ShiftMaster.IdentityService.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IdentityDbContext _context;

        public DepartmentRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync() =>
            await _context.Departments.ToListAsync();

        public async Task<Department?> GetByIdAsync(int id) =>
            await _context.Departments.FindAsync(id);

        public async Task AddAsync(Department department) =>
            await _context.Departments.AddAsync(department);

        public void Remove(Department department) =>
            _context.Departments.Remove(department);

        public async Task<bool> HasLinkedUsersAsync(int departmentId) =>
            await _context.Users.AnyAsync(u => u.DepartmentID == departmentId);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
