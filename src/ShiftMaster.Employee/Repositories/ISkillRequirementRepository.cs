using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Models;

namespace ShiftMaster.Employee.Repositories
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
