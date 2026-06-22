using System.Collections.Generic;
using System.Threading.Tasks;
using shiftmaster.models;

namespace Services.Interfaces.Repositories
{
    public interface ISkillRequirementRepository
    {
        Task<IEnumerable<SkillRequirement>> GetAllWithIncludesAsync();
        Task<SkillRequirement?> GetByIdWithIncludesAsync(int id);
        Task<SkillRequirement?> GetByIdAsync(int id);
        Task AddAsync(SkillRequirement requirement);
        void Remove(SkillRequirement requirement);
        Task SaveChangesAsync();
    }
}