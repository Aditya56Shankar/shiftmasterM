using System;
using System.Collections.Generic;
using System.Text;
using Services.Interfaces;
using Data.Context;
using shiftmaster.models;

namespace Services.Implementation
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
            await db.SaveChangesAsync();
            return roster;
        }
    }

}
