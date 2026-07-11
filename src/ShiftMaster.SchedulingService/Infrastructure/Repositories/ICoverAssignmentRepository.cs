using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Clients;
using ShiftMaster.SchedulingService.Domain.Models;

namespace ShiftMaster.SchedulingService.Infrastructure.Repositories
{
    public interface ICoverAssignmentRepository
    {
        Task<ShiftAssignment?> GetShiftAssignmentWithRosterAsync(int shiftAssignmentId);
        Task<List<string>> GetRequiredSkillNamesAsync(int locationId, int departmentId);
        Task<List<UserShortDto>> GetActiveUsersByLocationExceptAsync(int locationId, int excludedUserId);
        Task<bool> HasApprovedLeaveBlockOnDateAsync(int userId, DateTime date);
        Task<bool> HasConflictingShiftOnDateAsync(int userId, DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<List<string>> GetActiveUserSkillNamesAsync(int userId);
        Task<CoverAssignment> AddCoverAssignmentAsync(CoverAssignment coverAssignment);
        Task<CoverAssignment?> GetCoverAssignmentByIdAsync(int coverId);
        Task<ShiftAssignment?> GetShiftAssignmentByIdAsync(int assignmentId);
        Task SaveChangesAsync();
    }
}
