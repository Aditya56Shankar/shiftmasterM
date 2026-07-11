using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.Employee.Data;
using ShiftMaster.Employee.DTOs;
using ShiftMaster.Employee.Models;
using ShiftMaster.Employee.Enums;
using ShiftMaster.Employee.Repositories;
using ShiftMaster.Employee.Clients;

namespace ShiftMaster.Employee.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository repository;
        private readonly ISchedulingClient schedulingClient;
        private readonly IIdentityClient identityClient;
        private readonly EmployeeDbContext dbContext;

        public AvailabilityService(
            IAvailabilityRepository repository,
            ISchedulingClient schedulingClient,
            IIdentityClient identityClient,
            EmployeeDbContext dbContext)
        {
            this.repository = repository;
            this.schedulingClient = schedulingClient;
            this.identityClient = identityClient;
            this.dbContext = dbContext;
        }

        public async Task<EmployeeScheduleDto> GetMyScheduleAsync(int userId)
        {
            var shifts = await schedulingClient.GetShiftsByUserIdAsync(userId);
            var availabilities = await repository.GetByUserIdAsync(userId);

            // Fetch employee name from Identity Service
            var namesDict = await identityClient.GetUserNamesAsync(new List<int> { userId });
            var employeeName = namesDict.TryGetValue(userId, out var name) ? name : "Unknown";

            return new EmployeeScheduleDto
            {
                UserId = userId,
                EmployeeName = employeeName,
                Shifts = shifts,
                Availabilities = availabilities.Select(a =>
                    new EmployeeAvailabilityDto
                    {
                        AvailabilityId = a.AvailabilityID,
                        WeekStartDate = a.WeekStartDate,
                        AvailableDays = a.AvailableDays,
                        PreferredShiftType = a.PreferredShiftType,
                        Status = a.Status.ToString()
                    }).ToList()
            };
        }

        public async Task<AvailabilitySubmission> AddAvailableAsync(AvailabilitySubmission avail)
        {
            var startOfCurrentWeek = DateTime.UtcNow.Date
                .AddDays(-(int)DateTime.UtcNow.DayOfWeek);

            if (avail.WeekStartDate.Date < startOfCurrentWeek)
            {
                throw new Exception(
                    $"Availability submission failed. WeekStartDate '{avail.WeekStartDate:yyyy-MM-dd}' is in a previous week.");
            }

            avail.Status = AvailabilityStatus.Submitted;
            avail.SubmittedDate = DateTime.UtcNow;

            await repository.AddAsync(avail);
            await repository.SaveAsync();

            return avail;
        }

        public async Task<bool> UpdateAvailabilityStatusAsync(int id, AvailabilityStatus status)
        {
            var availability = await repository.GetByIdAsync(id);

            if (availability == null)
            {
                throw new Exception(
                    $"Availability record with ID {id} was not found.");
            }

            if (availability.Status == status)
            {
                throw new Exception(
                    $"Availability status is already '{status}'.");
            }

            availability.Status = status;
            await repository.SaveAsync();

            return true;
        }

        public async Task<bool> IsConfirmedAsync(int userId, DateTime date)
        {
            return await dbContext.AvailabilitySubmissions
                .AnyAsync(a =>
                    a.UserID == userId &&
                    a.Status == AvailabilityStatus.Acknowledged &&
                    a.WeekStartDate <= date &&
                    a.WeekStartDate.AddDays(6) >= date);
        }

        public async Task<AvailabilitySubmission?> GetAvailabilityAsync(int userId, DateTime targetDate)
        {
            return await repository.GetAvailabilityAsync(userId, targetDate);
        }
    }
}
