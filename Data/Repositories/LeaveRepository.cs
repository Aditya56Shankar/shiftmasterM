using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using shiftmaster.models;

public class LeaveRepository : ILeaveRepository
{
    private readonly ApplicationDbContext _context;

    public LeaveRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaveBlock>> GetActiveLeavesAsync(int userId)
    {
        return await _context.LeaveBlocks
            .Where(lb =>
                lb.UserID == userId &&
                lb.Status == LeaveStatus.Active)
            .ToListAsync();
    }
}