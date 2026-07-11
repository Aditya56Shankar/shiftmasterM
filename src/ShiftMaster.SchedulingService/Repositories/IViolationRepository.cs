using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Models;

namespace ShiftMaster.SchedulingService.Repositories
{
    public interface IViolationRepository
    {
        Task<List<SchedulingConstraintViolation>> GetExistingViolationsAsync(int rosterId, int userId);
        Task AddViolationAsync(SchedulingConstraintViolation violation);
        Task RemoveRangeAsync(List<SchedulingConstraintViolation> violations);
        Task<bool> HasBlockingViolationAsync(int rosterId, int userId);
        Task SaveAsync();
    }
}
