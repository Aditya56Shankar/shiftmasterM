using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using shiftmaster.models;

namespace Data.Repositories
{
    public class ShiftPatternRepository : IShiftPatternRepository
    {
        private readonly ApplicationDbContext _context;

        public ShiftPatternRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShiftPattern>> GetAllAsync()
        {
            return await _context.ShiftPatterns.ToListAsync();
        }

        public async Task<ShiftPattern?> GetByIdAsync(int id)
        {
            return await _context.ShiftPatterns.FindAsync(id);
        }

        // Changed from HasLinkedUsersAsync to match the interface requirement
        public async Task<bool> HasLinkedAssignmentsAsync(int id)
        {
            return await _context.ShiftPatterns.AnyAsync(p => p.PatternID == id);
        }

        public async Task AddAsync(ShiftPattern pattern)
        {
            await _context.ShiftPatterns.AddAsync(pattern);
        }

        public void Remove(ShiftPattern pattern)
        {
            _context.ShiftPatterns.Remove(pattern);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}