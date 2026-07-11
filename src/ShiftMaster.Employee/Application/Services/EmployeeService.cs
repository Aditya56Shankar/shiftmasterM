using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.Employee.Clients;
using ShiftMaster.Employee.Application.DTOs;
using ShiftMaster.Employee.Application.Interfaces;
using ShiftMaster.Employee.Domain.Enums;
using ShiftMaster.Employee.Infrastructure.Data;

namespace ShiftMaster.Employee.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityClient _identityClient;

        public EmployeeService(EmployeeDbContext context, IMapper mapper, IIdentityClient identityClient)
        {
            _context = context;
            _mapper = mapper;
            _identityClient = identityClient;
        }

        public async Task<List<EmployeeFullDto>> GetEmployeesFullData(int locationId, DateTime date)
        {
            // 1. Get user IDs in location from Identity Service
            var userIds = await _identityClient.GetUserIdsByLocationAsync(locationId);
            if (userIds == null || userIds.Count == 0)
                return new List<EmployeeFullDto>();

            // 2. Get user names from Identity Service
            var userNames = await _identityClient.GetUserNamesAsync(userIds);

            // 3. Get availabilities and skills for these users from local DB
            var dayName = date.DayOfWeek.ToString();
            var availabilities = await _context.AvailabilitySubmissions
                .Where(a => userIds.Contains(a.UserID) &&
                            a.WeekStartDate <= date &&
                            a.WeekStartDate.AddDays(6) >= date &&
                            a.AvailableDays.Contains(dayName))
                .ToListAsync();

            var activeUserIdsWithAvail = availabilities.Select(a => a.UserID).Distinct().ToList();

            var skills = await _context.EmployeeSkills
                .Where(s => activeUserIdsWithAvail.Contains(s.UserID) && s.Status == ActiveStatus.Active)
                .ToListAsync();

            var result = new List<EmployeeFullDto>();
            foreach (var userId in activeUserIdsWithAvail)
            {
                var name = userNames.TryGetValue(userId, out var n) ? n : "Unknown";
                var userAvails = availabilities.Where(a => a.UserID == userId).ToList();
                var userSkills = skills.Where(s => s.UserID == userId).ToList();

                result.Add(new EmployeeFullDto
                {
                    EmployeeId = userId,
                    Name = name,
                    Availability = _mapper.Map<List<AvailabilityDto>>(userAvails),
                    Skills = _mapper.Map<List<EmployeeSkillDto>>(userSkills)
                });
            }

            return result;
        }
    }
}
