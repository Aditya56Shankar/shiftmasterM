using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Domain.Models;
using ShiftMaster.IdentityService.Infrastructure.Data;

namespace ShiftMaster.IdentityService.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IdentityDbContext _context;

        public AuthRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByEmailWithRoleAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetUserIdByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .Select(u => (int?)u.UserID)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithDetailsByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.HomeLocation)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.UserID == id);
        }

        public async Task<bool> EmployeeIdExistsAsync(string employeeId)
        {
            return await _context.Users.AnyAsync(u => u.EmployeeID == employeeId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
