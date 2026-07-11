using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Clients;
using ShiftMaster.SchedulingService.Domain.Enums;
using ShiftMaster.SchedulingService.Infrastructure.Data;

namespace ShiftMaster.SchedulingService.Infrastructure.Repositories
{
    public class StatusCheckRepository : IStatusCheckRepository
    {
        private readonly SchedulingDbContext _context;
        private readonly IEmployeeClient _employeeClient;

        public StatusCheckRepository(SchedulingDbContext context, IEmployeeClient employeeClient)
        {
            _context = context;
            _employeeClient = employeeClient;
        }

        public async Task<bool> IsCoveredAsync(int userId)
        {
            return await _context.CoverAssignments
                .AnyAsync(c =>
                    c.CoveringUserID == userId &&
                    c.Status == CoverStatus.Completed);
        }

        public async Task<bool> IsSwappedAsync(int userId)
        {
            return await _context.SwapRequests
                .AnyAsync(s =>
                    (s.RequesterUserID == userId ||
                     s.TargetUserID == userId) &&
                    s.Status == ApprovalStatus.Approved);
        }

        public async Task<bool> IsConfirmedAsync(int userId, DateTime date)
        {
            return await _employeeClient.IsConfirmedAsync(userId, date);
        }
    }
}
