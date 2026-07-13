using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Clients;
using ShiftMaster.SchedulingService.Domain.Models;
using ShiftMaster.SchedulingService.Infrastructure.Data;

namespace ShiftMaster.SchedulingService.Infrastructure.Repositories
{
    public class WeeklyRosterRepository : IWeeklyRosterRepository
    {
        private readonly SchedulingDbContext db;
        private readonly IIdentityClient _identityClient;

        public WeeklyRosterRepository(SchedulingDbContext context, IIdentityClient identityClient)
        {
            db = context;
            _identityClient = identityClient;
        }

        public async Task<WeeklyRoster> AddAsync(WeeklyRoster roster)
        {
            await db.WeeklyRosters.AddAsync(roster);
            return roster;
        }

        public async Task<bool> LocationExistsAsync(int locationId)
        {
            return await _identityClient.LocationExistsAsync(locationId);
        }

        public async Task<bool> DepartmentExistsAsync(int departmentId)
        {
            return await _identityClient.DepartmentExistsAsync(departmentId);
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _identityClient.UserExistsAsync(userId);
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
            return await _identityClient.GetUserNamesAsync(userIds);
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
