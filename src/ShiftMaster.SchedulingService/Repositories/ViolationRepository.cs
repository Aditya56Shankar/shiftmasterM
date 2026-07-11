using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Data;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Enums;

namespace ShiftMaster.SchedulingService.Repositories
{
    public class ViolationRepository : IViolationRepository
    {
        private readonly SchedulingDbContext _context;

        public ViolationRepository(SchedulingDbContext context)
        {
            _context = context;
        }

        public async Task<List<SchedulingConstraintViolation>> GetExistingViolationsAsync(int rosterId, int userId)
        {
            return await _context.SchedulingConstraintViolations
                .Where(v =>
                    v.RosterID == rosterId &&
                    v.UserID == userId)
                .ToListAsync();
        }

        public async Task AddViolationAsync(SchedulingConstraintViolation violation)
        {
            await _context.SchedulingConstraintViolations.AddAsync(violation);
        }

        public async Task RemoveRangeAsync(List<SchedulingConstraintViolation> violations)
        {
            _context.SchedulingConstraintViolations.RemoveRange(violations);
            await Task.CompletedTask;
        }

        public async Task<bool> HasBlockingViolationAsync(int rosterId, int userId)
        {
            return await _context.SchedulingConstraintViolations
                .AnyAsync(v =>
                    v.RosterID == rosterId &&
                    v.UserID == userId &&
                    v.Status == ViolationStatus.Open &&
                    v.Severity == SeverityLevel.Blocking);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
