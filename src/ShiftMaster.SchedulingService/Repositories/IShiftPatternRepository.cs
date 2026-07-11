using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Models;

namespace ShiftMaster.SchedulingService.Repositories
{
    public interface IShiftPatternRepository
    {
        Task<IEnumerable<ShiftPattern>> GetAllAsync();
        Task<ShiftPattern?> GetByIdAsync(int id);
        Task AddAsync(ShiftPattern pattern);
        Task<bool> HasLinkedAssignmentsAsync(int id);
        void Remove(ShiftPattern pattern);
        Task SaveChangesAsync();
    }
}
