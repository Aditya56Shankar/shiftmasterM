using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.models;

namespace Services.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task AddAsync(Role role);
        void Remove(Role role);
        Task<bool> HasLinkedUsersAsync(int roleId);
        Task SaveChangesAsync();
    }
}