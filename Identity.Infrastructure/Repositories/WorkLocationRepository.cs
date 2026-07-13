using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Domain.Models;
using ShiftMaster.IdentityService.Infrastructure.Data;

namespace ShiftMaster.IdentityService.Infrastructure.Repositories
{
    public class WorkLocationRepository : IWorkLocationRepository
    {
        private readonly IdentityDbContext _context;

        public WorkLocationRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkLocation>> GetAllAsync() =>
            await _context.WorkLocations.ToListAsync();

        public async Task<WorkLocation?> GetByIdAsync(int id) =>
            await _context.WorkLocations.FindAsync(id);

        public async Task AddAsync(WorkLocation location) =>
            await _context.WorkLocations.AddAsync(location);

        public void Update(WorkLocation location) =>
            _context.WorkLocations.Update(location);

        public void Remove(WorkLocation location) =>
            _context.WorkLocations.Remove(location);

        public async Task<bool> HasLinkedUsersAsync(int locationId) =>
            await _context.Users.AnyAsync(u => u.LocationID == locationId);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
