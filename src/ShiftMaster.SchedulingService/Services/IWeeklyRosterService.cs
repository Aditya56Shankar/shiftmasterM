using System;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.DTOs;

namespace ShiftMaster.SchedulingService.Services
{
    public interface IWeeklyRosterService
    {
        Task<bool> UpdateRosterStatusAsync(int id, string action, int userId);
        Task<WeeklyRoster> AddAsync(WeeklyRoster roster);
        Task<SupervisorRosterResponseDto?> GetRosterAsync(int locationId, DateTime weekStartDate);
    }
}
