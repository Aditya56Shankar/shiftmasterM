using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using shiftmaster.models;

namespace Data.Repositories
{
    public class SkillRequirementRepository : ISkillRequirementRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRequirementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SkillRequirement>> GetAllWithIncludesAsync()
        {
            return await _context.SkillRequirements
                .Include(sr => sr.Location)
                .Include(sr => sr.Department)
                .ToListAsync();
        }

        public async Task<SkillRequirement?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.SkillRequirements
                .Include(sr => sr.Location)
                .Include(sr => sr.Department)
                .FirstOrDefaultAsync(req => req.SkillReqID == id);
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