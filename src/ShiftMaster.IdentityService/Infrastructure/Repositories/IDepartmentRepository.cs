using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Domain.Models;

namespace ShiftMaster.IdentityService.Infrastructure.Repositories
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
