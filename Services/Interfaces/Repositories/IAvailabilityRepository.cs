using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<AvailabilitySubmission> AddAsync(AvailabilitySubmission avail);
        Task<AvailabilitySubmission?> GetByIdAsync(int id);   

        Task<AvailabilitySubmission> GetWeeklyAvailabilityAsync(int userId, DateTime assignedDate);

        Task<AvailabilitySubmission?> GetAvailabilityAsync(int userId, DateTime targetDate);

        Task SaveAsync();
    }
}
