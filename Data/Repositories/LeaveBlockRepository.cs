using Data.Context;
using Domain.Repositories;
using shiftmaster.models;
using Microsoft.EntityFrameworkCore;

    namespace Services.Implementation
    {
        public class LeaveBlockRepository : ILeaveBlockRepository
        {
            private readonly ApplicationDbContext db;

            public LeaveBlockRepository(ApplicationDbContext db)
            {
                this.db = db;
            }


            public async Task<LeaveBlock?> GetByIdAsync(int id)
            {
                return await db.LeaveBlocks.FindAsync(id);
            }

            public async Task<List<LeaveBlock>> GetByUserIdAsync(int userId)
            {
                return await db.LeaveBlocks
                    .Where(x => x.UserID == userId)
                    .OrderByDescending(x => x.StartDate)
                    .ToListAsync();
            }

            public async Task<List<LeaveBlock>> GetPendingByLocationAsync(int locationId)
            {
                return await db.LeaveBlocks
                    .Where(lb => lb.Status == Domain.Enums.LeaveStatus.Pending
                        && db.Users.Any(u => u.UserID == lb.UserID && u.LocationID == locationId))
                    .OrderBy(x => x.StartDate)
                    .ToListAsync();
            }

            public async Task<bool> UserExistsAsync(int userId)
            {
                return await db.Users.AnyAsync(u => u.UserID == userId);
            }

            public async Task<bool> LocationExistsAsync(int locationId)
            {
                return await db.WorkLocations.AnyAsync(l => l.LocationID == locationId);
            }


            public async Task<LeaveBlock> AddAsync(LeaveBlock leave)
            {
                await db.LeaveBlocks.AddAsync(leave);
                return leave;
            }

            public async Task SaveAsync()
            {
                await db.SaveChangesAsync();
            }
        }
    }