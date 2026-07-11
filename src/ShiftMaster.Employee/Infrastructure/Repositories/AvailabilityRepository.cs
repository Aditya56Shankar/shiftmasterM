using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.Employee.Domain.Enums;
using ShiftMaster.Employee.Domain.Models;
using ShiftMaster.Employee.Infrastructure.Data;

namespace ShiftMaster.Employee.Infrastructure.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly EmployeeDbContext db;

        public AvailabilityRepository(EmployeeDbContext db)
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
}
