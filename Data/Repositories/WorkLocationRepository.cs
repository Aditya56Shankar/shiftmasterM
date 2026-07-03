using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Domain.Repositories;
using shiftmaster.models;
using Microsoft.EntityFrameworkCore;
namespace Data.Repositories
{
    public class WorkLocationRepository : IWorkLocationRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkLocation>> GetAllAsync()
        {
            return await _context.WorkLocations.ToListAsync();
        }
        public async Task<WorkLocation?> GetByIdAsync(int id) =>
            await _context.WorkLocations.FindAsync(id);

        public async Task AddAsync(WorkLocation location) =>
            await _context.WorkLocations.AddAsync(location);

        public void Update(WorkLocation location) =>
            _context.WorkLocations.Update(location);

        public void Remove(WorkLocation location) =>
            _context.WorkLocations.Remove(location);

        public async Task<bool> HasLinkedUsersAsync(int locationId)
        {
            return await _context.Users.AnyAsync(u => u.LocationID == locationId);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
