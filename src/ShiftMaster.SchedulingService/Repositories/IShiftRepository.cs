using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Models;

namespace ShiftMaster.SchedulingService.Repositories
{
    public interface IShiftRepository
    {
        Task<List<ShiftAssignment>> GetByUserIdAsync(int userId);
        Task<ShiftAssignment?> GetShiftWithDetailsAsync(int assignmentId);
        Task<List<ShiftAssignment>> GetWeeklyShiftsAsync(int rosterId, int userId);
        Task<List<ShiftAssignment>> GetUserAssignmentsAsync(int rosterId, int userId);
        Task SaveAsync();
        Task<int> GetAssignedCountAsync(int rosterId, DateTime date, int patternId);
        Task AddAsync(ShiftAssignment assignment);
        Task<bool> ShiftExistsAsync(int userId, DateTime assignedDate, TimeSpan startTime, TimeSpan endTime);
    }
}
