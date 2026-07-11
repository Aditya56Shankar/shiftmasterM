using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShiftMaster.SchedulingService.Enums;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Repositories;
using ShiftMaster.SchedulingService.DTOs;
using ShiftMaster.SchedulingService.Exceptions;

namespace ShiftMaster.SchedulingService.Services
{
    public class WeeklyRosterService : IWeeklyRosterService
    {
        private readonly IWeeklyRosterRepository repository;
        private readonly IMapper mapper;

        public WeeklyRosterService(IWeeklyRosterRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> UpdateRosterStatusAsync(int id, string action, int userId)
        {
            var roster = await repository.GetRosterByIdAsync(id);

            if (roster == null)
            {
                throw new Exception($"Roster with ID {id} was not found.");
            }

            action = action?.ToLower();

            if (action == "publish")
            {
                var startOfCurrentWeek = DateTime.UtcNow.Date
                    .AddDays(-(int)DateTime.UtcNow.DayOfWeek);

                if (roster.WeekStartDate.Date < startOfCurrentWeek)
                {
                    throw new Exception("Cannot publish rosters for previous weeks.");
                }

                roster.Status = RosterStatus.Published;
                roster.PublishedDate = DateTime.UtcNow;
            }
            else if (action == "amend")
            {
                roster.Status = RosterStatus.Amended;
            }
            else
            {
                throw new Exception("Invalid action. Use 'publish' or 'amend'");
            }

            roster.ApprovedByUserID = userId;

            await repository.SaveAsync();
            return true;
        }

        public async Task<WeeklyRoster> AddAsync(WeeklyRoster roster)
        {
            if (roster.LocationID <= 0)
                throw new ResourceNotFoundException("Invalid Location ID.");

            if (roster.DepartmentID <= 0)
                throw new ResourceNotFoundException("Invalid Department ID.");

            if (roster.CreatedByID <= 0)
                throw new ResourceNotFoundException("Invalid Created By User ID.");

            var locationExists = await repository.LocationExistsAsync(roster.LocationID);
            if (!locationExists)
                throw new ResourceNotFoundException($"Location {roster.LocationID} does not exist.");

            var departmentExists = await repository.DepartmentExistsAsync(roster.DepartmentID);
            if (!departmentExists)
                throw new ResourceNotFoundException($"Department {roster.DepartmentID} does not exist.");

            var userExists = await repository.UserExistsAsync(roster.CreatedByID);
            if (!userExists)
                throw new ResourceNotFoundException($"User {roster.CreatedByID} does not exist.");

            var startOfCurrentWeek = DateTime.UtcNow.Date
                .AddDays(-(int)DateTime.UtcNow.DayOfWeek);

            if (roster.WeekStartDate.Date < startOfCurrentWeek)
            {
                throw new InvalidWorkflowStateException("Cannot create rosters for previous weeks.");
            }

            await repository.AddAsync(roster);
            await repository.SaveAsync();

            return roster;
        }

        public async Task<SupervisorRosterResponseDto?> GetRosterAsync(int locationId, DateTime weekStartDate)
        {
            var roster = await repository.GetRosterEntityAsync(locationId, weekStartDate);

            if (roster == null)
            {
                throw new Exception($"No roster found for Location ID {locationId} and Week Start Date {weekStartDate:yyyy-MM-dd}.");
            }

            var shiftAssignments = roster.ShiftAssignments ?? new List<ShiftAssignment>();

            var userIds = shiftAssignments
                .Select(sa => sa.UserID)
                .Distinct()
                .ToList();

            var users = await repository.GetUserNamesAsync(userIds);

            var assignmentDtos = mapper.Map<List<SupervisorAssignmentViewDto>>(shiftAssignments);

            foreach (var a in assignmentDtos)
            {
                a.EmployeeName = users.TryGetValue(a.UserID, out var name)
                    ? name
                    : "Unknown Employee";
            }

            var response = mapper.Map<SupervisorRosterResponseDto>(roster);

            response.ShiftAssignments = assignmentDtos;
            response.Violations = mapper.Map<List<ViolationViewDto>>(
                roster.Violations ?? new List<SchedulingConstraintViolation>());

            UpdateRosterStatus(roster);

            return response;
        }

        private void UpdateRosterStatus(WeeklyRoster roster)
        {
            var today = DateTime.UtcNow.Date;

            if (roster.PublishedDate != default)
            {
                roster.Status = RosterStatus.Published;
                return;
            }

            if (roster.ApprovedByUserID == null)
            {
                roster.Status = RosterStatus.PendingApproval;
                return;
            }

            if (today < roster.WeekStartDate)
            {
                roster.Status = RosterStatus.Draft;
                return;
            }

            roster.Status = RosterStatus.Amended;
        }
    }
}
