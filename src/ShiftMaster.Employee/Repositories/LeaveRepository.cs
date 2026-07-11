using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.Employee.Data;
using ShiftMaster.Employee.Models;
using ShiftMaster.Employee.Enums;

namespace ShiftMaster.Employee.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly EmployeeDbContext _context;

        public LeaveRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveBlock>> GetActiveLeavesAsync(int userId)
        {
            return await _context.LeaveBlocks
                .Where(lb => lb.UserID == userId && lb.Status == LeaveStatus.Active)
                .ToListAsync();
        }
    }
}
