using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;

namespace Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(int departmentId);
        Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto);
        Task<bool> DeleteDepartmentAsync(int id);
        Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto newDepartment);
    }
}