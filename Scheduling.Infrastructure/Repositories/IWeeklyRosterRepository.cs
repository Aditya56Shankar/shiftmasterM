using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Domain.Models;

namespace ShiftMaster.SchedulingService.Infrastructure.Repositories
{
    public interface IWeeklyRosterRepository
    {
        Task<WeeklyRoster> AddAsync(WeeklyRoster roster);
        Task<bool> LocationExistsAsync(int locationId);
        Task<bool> DepartmentExistsAsync(int departmentId);
        Task<bool> UserExistsAsync(int userId);
        Task<WeeklyRoster?> GetRosterEntityAsync(int locationId, DateTime weekStartDate);
        Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds);
        Task SaveAsync();
        Task<WeeklyRoster?> GetRosterByIdAsync(int id);
    }
}
