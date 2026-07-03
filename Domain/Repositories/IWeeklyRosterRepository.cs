using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Domain.Repositories
{
    public interface IWeeklyRosterRepository
    {
        Task<WeeklyRoster> AddAsync(WeeklyRoster roster);
        Task<WeeklyRoster?> GetRosterEntityAsync(int locationId, DateTime weekStartDate);
        Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds);

        Task<WeeklyRoster?> GetRosterByIdAsync(int id);

        Task SaveAsync();
    }
}
