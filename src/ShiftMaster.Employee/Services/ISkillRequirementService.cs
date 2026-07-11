using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.DTOs;

namespace ShiftMaster.Employee.Services
{
    public interface ISkillRequirementService
    {
        Task<IEnumerable<SkillRequirementDto>> GetAllRequirementsAsync();
        Task<SkillRequirementDto?> GetRequirementByIdAsync(int skillReqId);
        Task<SkillRequirementDto?> UpdateRequirementAsync(int id, UpdateSkillRequirementDto dto);
        Task<bool> DeleteRequirementAsync(int id);
        Task<SkillRequirementDto> CreateRequirementAsync(CreateSkillRequirementDto newReq);
    }
}
