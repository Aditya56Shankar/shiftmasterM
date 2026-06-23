using Services.DTOs;
using shiftmaster.models;

public interface IWeeklyRosterService
{
    Task<WeeklyRoster> AddAsync(WeeklyRoster roster);
    Task<SupervisorRosterResponseDto?> GetRosterAsync(int locationId, DateTime weekStartDate);
    Task<bool> UpdateRosterStatusAsync(int id, string action, int userId);
}