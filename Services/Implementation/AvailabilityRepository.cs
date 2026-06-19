using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Data.Context;
using Domain.Enums;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly ApplicationDbContext db;
        public AvailabilityRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<AvailabilitySubmission> AddAvailableAsync(AvailabilitySubmission avail)
        {
            avail.Status = AvailabilityStatus.Submitted;
            avail.SubmittedDate = DateTime.UtcNow;

            await db.AvailabilitySubmissions.AddAsync(avail);
            await db.SaveChangesAsync();

            return avail;
        }

        public async Task<AvailabilitySubmission> CheckAndUpdateAvailabilityAsync(ShiftAssignment assignment)
        {
            // Get employee availability for that week
            var availability = db.AvailabilitySubmissions
                .FirstOrDefault(a =>
                    a.UserID == assignment.UserID &&
                    a.WeekStartDate <= assignment.AssignedDate &&
                    a.WeekStartDate.AddDays(7) > assignment.AssignedDate);

            if (availability == null)
                return null;

            bool isMatch = true;

            // ✅ Check day availability
            var assignedDay = assignment.AssignedDate.DayOfWeek.ToString();

            if (!availability.AvailableDays.Contains(assignedDay))
                isMatch = false;

            // ✅ Check shift type (example logic)
            if (!string.IsNullOrEmpty(availability.PreferredShiftType))
            {
                if (availability.PreferredShiftType != assignment.Role) // adjust if needed
                    isMatch = false;
            }

            // ✅ OPTIONAL: Check hours (basic logic)
            var shiftHours = (decimal)(assignment.EndTime - assignment.StartTime).TotalHours;
            if (shiftHours > availability.MaxHoursPerWeek)
                isMatch = false;

            // ✅ Set status based on result
            availability.Status = isMatch
                ? AvailabilityStatus.Acknowledged
                : AvailabilityStatus.Overridden;

            await db.SaveChangesAsync();

            return availability;
        }

    }
}
