using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Domain.Models;

namespace ShiftMaster.Employee.Infrastructure.Repositories
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
