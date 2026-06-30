using Data.Context;
using Domain.Repositories;
using shiftmaster.models;

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