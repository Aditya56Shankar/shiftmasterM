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


    public async Task<AvailabilitySubmission?> GetAvailabilityAsync(int userId, DateTime targetDate)
    {
        return await db.AvailabilitySubmissions
            .FirstOrDefaultAsync(a =>
                a.UserID == userId &&
                a.WeekStartDate <= targetDate &&
                a.WeekStartDate.AddDays(6) >= targetDate &&
                a.Status == AvailabilityStatus.Submitted);
    }



    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }

    public Task<AvailabilitySubmission> GetWeeklyAvailabilityAsync(int userId, DateTime assignedDate)
    {
        throw new NotImplementedException();
    }
}