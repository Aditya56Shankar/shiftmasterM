using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.models;

namespace Services.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task AddAsync(Department department);
        void Remove(Department department);
        Task<bool> HasLinkedUsersAsync(int departmentId);
        Task SaveChangesAsync();
    }
}