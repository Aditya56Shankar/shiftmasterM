using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Services.DTOs;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using shiftmaster.models;

namespace Services.Implementation
{
    public class SkillRequirementService : ISkillRequirementService
    {
        private readonly ISkillRequirementRepository _repo;
        private readonly IMapper _mapper;

        public SkillRequirementService(ISkillRequirementRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SkillRequirementDto>> GetAllRequirementsAsync()
        {
            var requirements = await _repo.GetAllWithIncludesAsync();
            return _mapper.Map<IEnumerable<SkillRequirementDto>>(requirements);
        }

        public async Task<SkillRequirementDto?> GetRequirementByIdAsync(int skillReqId)
        {
            var sr = await _repo.GetByIdWithIncludesAsync(skillReqId);
            return _mapper.Map<SkillRequirementDto>(sr);
        }

        public async Task<SkillRequirementDto?> UpdateRequirementAsync(int id, UpdateSkillRequirementDto dto)
        {
            var req = await _repo.GetByIdAsync(id);
            if (req == null) return null;

            _mapper.Map(dto, req);
            await _repo.SaveChangesAsync();

            return new SkillRequirementDto
            {
                SkillReqID = req.SkillReqID,
                SkillName = req.SkillName
            };
        }

        public async Task<bool> DeleteRequirementAsync(int id)
        {
            var req = await _repo.GetByIdAsync(id);
            if (req == null) return false;

            _repo.Remove(req);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<SkillRequirementDto> CreateRequirementAsync(CreateSkillRequirementDto newReq)
        {
            var requirement = _mapper.Map<SkillRequirement>(newReq);
            await _repo.AddAsync(requirement);
            await _repo.SaveChangesAsync();

            var savedRequirement = await _repo.GetByIdWithIncludesAsync(requirement.SkillReqID);
            return _mapper.Map<SkillRequirementDto>(savedRequirement ?? requirement);
        }
    }
}