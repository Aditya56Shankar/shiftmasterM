using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.Employee.Clients;
using ShiftMaster.Employee.Domain.Enums;
using ShiftMaster.Employee.Domain.Models;
using ShiftMaster.Employee.Infrastructure.Data;

namespace ShiftMaster.Employee.Infrastructure.Repositories
{
    public class LeaveBlockRepository : ILeaveBlockRepository
    {
        private readonly EmployeeDbContext db;
        private readonly IIdentityClient _identityClient;

        public LeaveBlockRepository(EmployeeDbContext db, IIdentityClient identityClient)
        {
            this.db = db;
            _identityClient = identityClient;
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
            var userIds = await _identityClient.GetUserIdsByLocationAsync(locationId);
            return await db.LeaveBlocks
                .Where(lb => lb.Status == LeaveStatus.Pending
                    && userIds.Contains(lb.UserID))
                .OrderBy(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _identityClient.UserExistsAsync(userId);
        }

        public async Task<bool> LocationExistsAsync(int locationId)
        {
            return await _identityClient.LocationExistsAsync(locationId);
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
