using System;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Application.DTOs;
using ShiftMaster.SchedulingService.Domain.Models;

namespace ShiftMaster.SchedulingService.Application.Interfaces
{
    public interface IWeeklyRosterService
    {
        Task<bool> UpdateRosterStatusAsync(int id, string action, int userId);
        Task<WeeklyRoster> AddAsync(WeeklyRoster roster);
        Task<SupervisorRosterResponseDto?> GetRosterAsync(int locationId, DateTime weekStartDate);
    }
}
