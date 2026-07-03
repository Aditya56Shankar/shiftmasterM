using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;

public class StatusCheckRepository : IStatusCheckRepository
{
    private readonly ApplicationDbContext _context;

    public StatusCheckRepository(ApplicationDbContext context)
    {
        _context = context;
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
        return await _context.AvailabilitySubmissions
            .AnyAsync(a =>
                a.UserID == userId &&
                a.Status == AvailabilityStatus.Acknowledged &&
                a.WeekStartDate <= date &&
                a.WeekStartDate.AddDays(6) >= date);
    }
}