using AutoMapper;
using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementation
{

    public class WeeklyRosterRepository : IWeeklyRosterRepository
    {
        private readonly ApplicationDbContext db;


        private readonly IMapper mapper; 
        public WeeklyRosterRepository(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            this.mapper = mapper;
        }



        public async Task<WeeklyRoster> AddAsync(WeeklyRoster roster)
        {
            var today = DateTime.UtcNow.Date;

            if (today < roster.WeekStartDate)
            {
                roster.Status = RosterStatus.Draft;
            }
            else
            {
                roster.Status = RosterStatus.Published;
            }

            await db.WeeklyRosters.AddAsync(roster);
            await db.SaveChangesAsync();
            return roster;
        }


        public async Task<SupervisorRosterResponseDto?> GetRosterAsync(int locationId, DateTime weekStartDate)
        {
            var roster = await db.WeeklyRosters
                .AsSplitQuery()
                .Include(r => r.ShiftAssignments)
                .Include(r => r.Violations)
                .FirstOrDefaultAsync(r =>
                    r.LocationID == locationId &&
                    r.WeekStartDate == weekStartDate.Date);

            if (roster == null)
                return null;

            var shiftAssignments = roster.ShiftAssignments ?? new List<ShiftAssignment>();

            // ✅ Get user IDs
            var userIds = shiftAssignments
                .Select(sa => sa.UserID)
                .Distinct()
                .ToList();

            // ✅ Fetch user names
            var users = await db.Users
                .Where(u => userIds.Contains(u.UserID))
                .ToDictionaryAsync(u => u.UserID, u => u.Name);

            // ✅ Map assignments
            var assignmentDtos = mapper.Map<List<SupervisorAssignmentViewDto>>(shiftAssignments);

            foreach (var a in assignmentDtos)
            {
                a.EmployeeName = users.TryGetValue(a.UserID, out var name)
                    ? name
                    : "Unknown Employee";
            }

            // ✅ Map main response
            var response = mapper.Map<SupervisorRosterResponseDto>(roster);

            response.ShiftAssignments = assignmentDtos;
            response.Violations = mapper.Map<List<ViolationViewDto>>(roster.Violations ?? new List<SchedulingConstraintViolation>());
            UpdateRosterStatus(roster);
            return response;
        }
        private void UpdateRosterStatus(WeeklyRoster roster)
        {
            var today = DateTime.UtcNow.Date;

            // ✅ If week not started 
            if (today < roster.WeekStartDate)
            {
                roster.Status = RosterStatus.Draft;
                return;
            }

            // ✅ If week started -> Published
            if (today >= roster.WeekStartDate && roster.Status != RosterStatus.Amended)
            {
                roster.Status = RosterStatus.Published;
                return;
            }
        }
    }

}
