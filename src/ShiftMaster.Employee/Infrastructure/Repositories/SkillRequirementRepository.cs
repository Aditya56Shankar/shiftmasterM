using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.Employee.Domain.Models;
using ShiftMaster.Employee.Infrastructure.Data;

namespace ShiftMaster.Employee.Infrastructure.Repositories
{
    public class SkillRequirementRepository : ISkillRequirementRepository
    {
        private readonly EmployeeDbContext _context;

        public SkillRequirementRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SkillRequirement>> GetAllWithIncludesAsync()
        {
            return await _context.SkillRequirements.ToListAsync();
        }

        public async Task<SkillRequirement?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.SkillRequirements.FirstOrDefaultAsync(req => req.SkillReqID == id);
        }

        public async Task<SkillRequirement?> GetByIdAsync(int id)
        {
            return await _context.SkillRequirements.FindAsync(id);
        }

        public async Task AddAsync(SkillRequirement requirement)
        {
            await _context.SkillRequirements.AddAsync(requirement);
        }

        public void Remove(SkillRequirement requirement)
        {
            _context.SkillRequirements.Remove(requirement);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
