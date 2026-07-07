using System;
using System.Collections.Generic;
using System.Text;
using Data.Repositories;
using Domain.Enums;
using Domain.Repositories;
using Services.Interfaces;
using shiftmaster.models;
using shiftMaster.Services.DTOs;

namespace Services.Implementation
{
    public class AvailabilityService : IAvailabilityService
    {

        private readonly IAvailabilityRepository repository;
        private readonly IShiftRepository shiftRepository;

        public AvailabilityService(
            IAvailabilityRepository repository,
            IShiftRepository shiftRepository)
        {
            this.repository = repository;
            this.shiftRepository = shiftRepository;
        }
        public async Task<EmployeeScheduleDto> GetMyScheduleAsync(int userId)
        {
            var shifts = await shiftRepository.GetByUserIdAsync(userId);

            var availabilities = await repository.GetByUserIdAsync(userId);

            return new EmployeeScheduleDto
            {
                UserId = userId,

                Shifts = shifts.Select(s => new EmployeeShiftDto
                {
                    AssignmentId = s.AssignmentID,
                    AssignedDate = s.AssignedDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Status = s.Status.ToString()
                }).ToList(),

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

        public async Task<AvailabilitySubmission> CheckAndUpdateAvailabilityAsync(ShiftAssignment assignment)
        {
            var availability = await repository.GetWeeklyAvailabilityAsync(
                assignment.UserID, assignment.AssignedDate);

            if (availability == null)
            {
                throw new Exception(
                    $"No availability record found for User ID {assignment.UserID} on {assignment.AssignedDate:yyyy-MM-dd}.");
            }

            bool isMatch = true;

            var assignedDay = assignment.AssignedDate.DayOfWeek.ToString();

            if (!availability.AvailableDays.Contains(assignedDay))
                isMatch = false;

            if (!string.IsNullOrEmpty(availability.PreferredShiftType))
            {
                if (availability.PreferredShiftType != assignment.Role)
                    isMatch = false;
            }

            var shiftHours =
                (decimal)(assignment.EndTime - assignment.StartTime).TotalHours;

            if (shiftHours > availability.MaxHoursPerWeek)
                isMatch = false;

            availability.Status = isMatch
                ? AvailabilityStatus.Acknowledged
                : AvailabilityStatus.Overridden;

            await repository.SaveAsync();

            return availability;
        }

        public async Task<bool> UpdateAvailabilityStatusAsync(int id,AvailabilityStatus status)
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

    }
}
