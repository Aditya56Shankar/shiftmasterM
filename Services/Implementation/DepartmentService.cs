using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.models;
using Services.DTOs;
using Services.Interfaces;
using Domain.Repositories;

namespace Services.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repo;

        public DepartmentService(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var depts = await _repo.GetAllAsync();
            return depts.Select(d => MapToDto(d));
        }

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int departmentId)
        {
            var d = await _repo.GetByIdAsync(departmentId);
            return d == null ? null : MapToDto(d);
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto newDepartment)
        {
            var department = new Department
            {
                departmentName = newDepartment.DepartmentName
            };

            await _repo.AddAsync(department);
            await _repo.SaveChangesAsync();

            return MapToDto(department);
        }

        public async Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
        {
            var dept = await _repo.GetByIdAsync(id);
            if (dept == null) return null;

            dept.departmentName = dto.DepartmentName;
            await _repo.SaveChangesAsync();

            return MapToDto(dept);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var dept = await _repo.GetByIdAsync(id);
            if (dept == null) return false;

            if (await _repo.HasLinkedUsersAsync(id))
                throw new InvalidOperationException("Cannot delete department with active employees allocated.");

            _repo.Remove(dept);
            await _repo.SaveChangesAsync();
            return true;
        }

        private static DepartmentDto MapToDto(Department d) => new()
        {
            DepartmentId = d.departmentId,
            DepartmentName = d.departmentName
        };
    }
}