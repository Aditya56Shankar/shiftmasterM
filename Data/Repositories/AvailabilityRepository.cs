using Data.Context;
using Domain.Enums;
using Domain.Repositories;
using shiftmaster.models;
using Microsoft.EntityFrameworkCore;
public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly ApplicationDbContext db;

    public AvailabilityRepository(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task<AvailabilitySubmission> AddAsync(AvailabilitySubmission avail)
    {
        await db.AvailabilitySubmissions.AddAsync(avail);
        return avail;
    }

    public async Task<AvailabilitySubmission?> GetByIdAsync(int id)
    {
        return await db.AvailabilitySubmissions.FindAsync(id);
    }


    public async Task<AvailabilitySubmission?> GetAvailabilityAsync(
    int userId,
    DateTime targetDate)
    {
        targetDate = targetDate.Date;

        return await db.AvailabilitySubmissions
            .FirstOrDefaultAsync(a =>
                a.UserID == userId &&
                a.Status == AvailabilityStatus.Submitted &&
                a.WeekStartDate.Date <= targetDate &&
                a.WeekStartDate.Date.AddDays(6) >= targetDate);
    }



    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }

    public async Task<List<AvailabilitySubmission>> GetByUserIdAsync(int userId)
    {
        return await db.AvailabilitySubmissions
            .Where(a => a.UserID == userId)
            .OrderByDescending(a => a.WeekStartDate)
            .ToListAsync();
    }

    public Task<AvailabilitySubmission?> GetWeeklyAvailabilityAsync(int userId, DateTime assignedDate)
    {
        return GetAvailabilityAsync(userId, assignedDate);

    }
}