using shiftmaster.models;

namespace Domain.Repositories
{
    public interface IWorkLocationRepository
    {
        Task<IEnumerable<WorkLocation>> GetAllAsync();
        Task<WorkLocation?> GetByIdAsync(int id);
        Task AddAsync(WorkLocation location);
        void Update(WorkLocation location); // EF tracks changes, so often synchronous or omitted depending on setup
        void Remove(WorkLocation location);
        Task<bool> HasLinkedUsersAsync(int locationId);
        Task SaveChangesAsync();
    }
}