using Data.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using shiftmaster.models;

namespace Data.Repositories
{
    public class WeeklyRosterRepository : IWeeklyRosterRepository
    {
        private readonly ApplicationDbContext db;

        public WeeklyRosterRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<WeeklyRoster> AddAsync(WeeklyRoster roster)
        {
            await db.WeeklyRosters.AddAsync(roster);
            return roster;
        }

        public async Task<WeeklyRoster?> GetRosterEntityAsync(int locationId, DateTime weekStartDate)
        {
            return await db.WeeklyRosters
                .AsSplitQuery()
                .Include(r => r.ShiftAssignments)
                .Include(r => r.Violations)
                .FirstOrDefaultAsync(r =>
                    r.LocationID == locationId &&
                    r.WeekStartDate == weekStartDate.Date);
        }

        public async Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds)
        {
            return await db.Users
                .Where(u => userIds.Contains(u.UserID))
                .ToDictionaryAsync(u => u.UserID, u => u.Name);
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public async Task<WeeklyRoster?> GetRosterByIdAsync(int id)
        {
            return await db.WeeklyRosters.FindAsync(id);
        }
    }
}