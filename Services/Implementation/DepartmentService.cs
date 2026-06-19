using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.models;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;

namespace Services.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .Select(d => new DepartmentDto
                {
                    DepartmentId = d.departmentId,
                    DepartmentName = d.departmentName
                }).ToListAsync();
        }

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int departmentId)
        {
            var d = await _context.Departments.FindAsync(departmentId);
            if (d == null) return null;

            return new DepartmentDto
            {
                DepartmentId = d.departmentId,
                DepartmentName = d.departmentName
            };
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto newDepartment)
        {
            var department = new Department
            {
                departmentName = newDepartment.DepartmentName
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                DepartmentId = department.departmentId,
                DepartmentName = department.departmentName
            };
        }
    }
}