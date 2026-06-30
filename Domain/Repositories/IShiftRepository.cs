using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Domain.Repositories
{
    public interface IShiftRepository
    {
        Task<ShiftAssignment> GetShiftWithDetailsAsync(int assignmentId);
        Task<List<ShiftAssignment>> GetWeeklyShiftsAsync(int rosterId, int userId);
        Task<int> GetAssignedCountAsync(int rosterId, DateTime date, int patternId);


        Task<List<ShiftAssignment>> GetUserAssignmentsAsync(int rosterId, int userId);
        Task SaveAsync();

    }

}
