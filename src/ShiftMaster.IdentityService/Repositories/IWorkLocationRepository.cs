using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Models;

namespace ShiftMaster.IdentityService.Repositories
{
    public interface IWorkLocationRepository
    {
        Task<IEnumerable<WorkLocation>> GetAllAsync();
        Task<WorkLocation?> GetByIdAsync(int id);
        Task AddAsync(WorkLocation location);
        void Update(WorkLocation location);
        void Remove(WorkLocation location);
        Task<bool> HasLinkedUsersAsync(int locationId);
        Task SaveChangesAsync();
    }
}
