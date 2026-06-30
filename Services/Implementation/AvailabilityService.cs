using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using Services.Interfaces;
using Domain.Repositories;
using shiftmaster.models;

namespace Services.Implementation
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository repository;

        public AvailabilityService(IAvailabilityRepository repository)
        {
            this.repository = repository;
        }

        public async Task<AvailabilitySubmission> AddAvailableAsync(AvailabilitySubmission avail)
        {
            avail.Status = AvailabilityStatus.Submitted;
            avail.SubmittedDate = DateTime.UtcNow;

            await repository.AddAsync(avail);
            await repository.SaveAsync();

            return avail;
        }

        public async Task<AvailabilitySubmission> CheckAndUpdateAvailabilityAsync(ShiftAssignment assignment)
        {
            var availability = await repository.GetWeeklyAvailabilityAsync(
                assignment.UserID, assignment.AssignedDate);

            if (availability == null)
                return null;

            bool isMatch = true;

            
            var assignedDay = assignment.AssignedDate.DayOfWeek.ToString();
            if (!availability.AvailableDays.Contains(assignedDay))
                isMatch = false;

            
            if (!string.IsNullOrEmpty(availability.PreferredShiftType))
            {
                if (availability.PreferredShiftType != assignment.Role)
                    isMatch = false;
            }

            
            var shiftHours = (decimal)(assignment.EndTime - assignment.StartTime).TotalHours;
            if (shiftHours > availability.MaxHoursPerWeek)
                isMatch = false;

           
            availability.Status = isMatch
                ? AvailabilityStatus.Acknowledged
                : AvailabilityStatus.Overridden;

            await repository.SaveAsync();

            return availability;
        }

        public async Task<bool> UpdateAvailabilityStatusAsync(int id, AvailabilityStatus status)
        {
            var availability = await repository.GetByIdAsync(id);

            if (availability == null)
                return false;

            if (availability.Status == status)
                throw new Exception("Status is already set to this value");

            availability.Status = status;

            await repository.SaveAsync();
            return true;
        }
        
   

    }
}
