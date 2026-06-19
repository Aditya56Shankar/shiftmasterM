using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;

namespace Services.Interfaces
{
    public interface ISkillRequirementService
    {
        Task<IEnumerable<SkillRequirementDto>> GetAllRequirementsAsync();
        Task<SkillRequirementDto?> GetRequirementByIdAsync(int skillReqId);
        Task<SkillRequirementDto> CreateRequirementAsync(CreateSkillRequirementDto newReq);
    }
}