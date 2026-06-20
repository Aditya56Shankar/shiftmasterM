using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    public class SkillRequirementService : ISkillRequirementService
    {
        private readonly ApplicationDbContext _context;

        public SkillRequirementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SkillRequirementDto>> GetAllRequirementsAsync()
        {
            return await _context.SkillRequirements
                .Include(sr => sr.Location)
                .Include(sr => sr.Department)
                .Select(sr => new SkillRequirementDto
                {
                    SkillReqID = sr.SkillReqID,
                    SkillName = sr.SkillName,
                    MinCountPerShift = sr.MinCountPerShift,
                    Status = sr.Status.ToString(),
                    LocationID = sr.LocationID,
                    LocationName = sr.Location != null ? sr.Location.LocationName : "Unassigned",
                    DepartmentID = sr.DepartmentID,
                    DepartmentName = sr.Department != null ? sr.Department.departmentName : "Unassigned"
                }).ToListAsync();
        }

        public async Task<SkillRequirementDto?> GetRequirementByIdAsync(int skillReqId)
        {
            var sr = await _context.SkillRequirements
                .Include(sr => sr.Location)
                .Include(sr => sr.Department)
                .FirstOrDefaultAsync(req => req.SkillReqID == skillReqId);

            if (sr == null) return null;

            return new SkillRequirementDto
            {
                SkillReqID = sr.SkillReqID,
                SkillName = sr.SkillName,
                MinCountPerShift = sr.MinCountPerShift,
                Status = sr.Status.ToString(),
                LocationID = sr.LocationID,
                LocationName = sr.Location != null ? sr.Location.LocationName : "Unassigned",
                DepartmentID = sr.DepartmentID,
                DepartmentName = sr.Department != null ? sr.Department.departmentName : "Unassigned"
            };
        }

        public async Task<SkillRequirementDto?> UpdateRequirementAsync(int id, UpdateSkillRequirementDto dto)
        {
            var req = await _context.SkillRequirements.FindAsync(id);
            if (req == null) return null;

            req.SkillName = dto.SkillName;
            req.MinCountPerShift = dto.MinCountPerShift;
            req.Status = Enum.Parse<Domain.Enums.ActiveStatus>(dto.Status, true);
            req.LocationID = dto.LocationID;
            req.DepartmentID = dto.DepartmentID;

            await _context.SaveChangesAsync();
            return new SkillRequirementDto { SkillReqID = req.SkillReqID, SkillName = req.SkillName };
        }

        public async Task<bool> DeleteRequirementAsync(int id)
        {
            var req = await _context.SkillRequirements.FindAsync(id);
            if (req == null) return false;

            _context.SkillRequirements.Remove(req);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SkillRequirementDto> CreateRequirementAsync(CreateSkillRequirementDto newReq)
        {
            var requirement = new SkillRequirement
            {
                SkillName = newReq.SkillName,
                MinCountPerShift = newReq.MinCountPerShift,
                Status = Enum.Parse<ActiveStatus>(newReq.Status, true),
                LocationID = newReq.LocationID,
                DepartmentID = newReq.DepartmentID
            };

            _context.SkillRequirements.Add(requirement);
            await _context.SaveChangesAsync();

            return await GetRequirementByIdAsync(requirement.SkillReqID)
                ?? new SkillRequirementDto { SkillReqID = requirement.SkillReqID, SkillName = requirement.SkillName };
        }
    }
}