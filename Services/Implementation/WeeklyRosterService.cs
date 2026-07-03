using AutoMapper;
using Domain.Enums;
using Services.DTOs;
using Services.Interfaces;
using Domain.Repositories;
using shiftmaster.models;

namespace Services.Implementation
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
                return false;

            action = action?.ToLower();

            if (action == "publish")
            {
                roster.Status = RosterStatus.Published;
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
            var today = DateTime.UtcNow.Date;

            // ✅ Business logic
            roster.Status = today < roster.WeekStartDate
                ? RosterStatus.Draft
                : RosterStatus.Published;

            await repository.AddAsync(roster);
            await repository.SaveAsync();

            return roster;
        }

        public async Task<SupervisorRosterResponseDto?> GetRosterAsync(int locationId, DateTime weekStartDate)
        {
            var roster = await repository.GetRosterEntityAsync(locationId, weekStartDate);

            if (roster == null)
                return null;

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

            if (roster.PublishedDate != null)
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