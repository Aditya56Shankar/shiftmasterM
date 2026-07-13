using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.Application.DTOs;

namespace ShiftMaster.IdentityService.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(int departmentId);
        Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto newDepartment);
        Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
