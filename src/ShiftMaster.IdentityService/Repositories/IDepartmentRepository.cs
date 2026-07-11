using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Models;

namespace ShiftMaster.IdentityService.Repositories
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
