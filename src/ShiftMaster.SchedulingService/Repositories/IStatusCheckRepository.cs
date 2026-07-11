using System;
using System.Threading.Tasks;

namespace ShiftMaster.SchedulingService.Repositories
{
    public interface IStatusCheckRepository
    {
        Task<bool> IsCoveredAsync(int userId);
        Task<bool> IsSwappedAsync(int userId);
        Task<bool> IsConfirmedAsync(int userId, DateTime date);
    }
}
