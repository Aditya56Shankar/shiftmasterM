using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Application.DTOs;
using ShiftMaster.IdentityService.Application.Interfaces;
using ShiftMaster.IdentityService.Infrastructure.Data;

namespace ShiftMaster.IdentityService.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IdentityDbContext _context;

        public DepartmentsController(IDepartmentService departmentService, IdentityDbContext context)
        {
            _departmentService = departmentService;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "HR")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();

            return Ok(department);
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto newDepartment)
        {
            var createdDepartment = await _departmentService.CreateDepartmentAsync(newDepartment);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = createdDepartment.DepartmentId }, createdDepartment);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int id, UpdateDepartmentDto dto)
        {
            var updated = await _departmentService.UpdateDepartmentAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var success = await _departmentService.DeleteDepartmentAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ==========================================
        // INTERNAL HELPER ENDPOINTS (UNAUTHENTICATED)
        // ==========================================

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> DepartmentExists(int id)
        {
            var exists = await _context.Departments.AnyAsync(d => d.departmentId == id);
            return Ok(exists);
        }

        [HttpGet("internal/{id}/name")]
        public async Task<IActionResult> GetDepartmentNameInternal(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();
            return Ok(dept.departmentName);
        }
    }
}
