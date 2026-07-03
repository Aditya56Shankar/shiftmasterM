using System.Threading.Tasks;
using Data.Context;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.models;
using Services.Interfaces.Repositories;

namespace Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailWithRoleAsync(string email)
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

        public async Task<User> GetUserWithDetailsByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.HomeLocation)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.UserID == id);
        }
    }
}