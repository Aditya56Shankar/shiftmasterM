using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Domain.Models;

namespace ShiftMaster.Employee.Infrastructure.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<AvailabilitySubmission> AddAsync(AvailabilitySubmission avail);
        Task<AvailabilitySubmission?> GetByIdAsync(int id);
        Task<AvailabilitySubmission?> GetWeeklyAvailabilityAsync(int userId, DateTime assignedDate);
        Task<List<AvailabilitySubmission>> GetByUserIdAsync(int userId);
        Task<AvailabilitySubmission?> GetAvailabilityAsync(int userId, DateTime targetDate);
        Task SaveAsync();
    }
}
