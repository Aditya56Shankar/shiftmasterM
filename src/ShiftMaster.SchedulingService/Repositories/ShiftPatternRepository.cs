using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Data;
using ShiftMaster.SchedulingService.Models;

namespace ShiftMaster.SchedulingService.Repositories
{
    public class ShiftPatternRepository : IShiftPatternRepository
    {
        private readonly SchedulingDbContext _context;

        public ShiftPatternRepository(SchedulingDbContext context)
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

        public async Task<bool> HasLinkedAssignmentsAsync(int id)
        {
            return await _context.ShiftAssignments.AnyAsync(sa => sa.ShiftPatternID == id);
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
