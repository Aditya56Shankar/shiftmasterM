using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShiftMaster.Employee.DTOs;
using ShiftMaster.Employee.Models;
using ShiftMaster.Employee.Repositories;
using ShiftMaster.Employee.Clients;

namespace ShiftMaster.Employee.Services
{
    public class SkillRequirementService : ISkillRequirementService
    {
        private readonly ISkillRequirementRepository _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityClient _identityClient;

        public SkillRequirementService(ISkillRequirementRepository repo, IMapper mapper, IIdentityClient identityClient)
        {
            _repo = repo;
            _mapper = mapper;
            _identityClient = identityClient;
        }

        public async Task<IEnumerable<SkillRequirementDto>> GetAllRequirementsAsync()
        {
            var requirements = await _repo.GetAllWithIncludesAsync();
            var dtos = _mapper.Map<List<SkillRequirementDto>>(requirements);
            foreach (var dto in dtos)
            {
                dto.LocationName = await _identityClient.GetLocationNameAsync(dto.LocationID) ?? "Unassigned";
                dto.DepartmentName = await _identityClient.GetDepartmentNameAsync(dto.DepartmentID) ?? "Unassigned";
            }
            return dtos;
        }

        public async Task<SkillRequirementDto?> GetRequirementByIdAsync(int skillReqId)
        {
            var sr = await _repo.GetByIdWithIncludesAsync(skillReqId);
            var dto = _mapper.Map<SkillRequirementDto>(sr);
            if (dto != null)
            {
                dto.LocationName = await _identityClient.GetLocationNameAsync(dto.LocationID) ?? "Unassigned";
                dto.DepartmentName = await _identityClient.GetDepartmentNameAsync(dto.DepartmentID) ?? "Unassigned";
            }
            return dto;
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

            var dto = _mapper.Map<SkillRequirementDto>(requirement);
            dto.LocationName = await _identityClient.GetLocationNameAsync(dto.LocationID) ?? "Unassigned";
            dto.DepartmentName = await _identityClient.GetDepartmentNameAsync(dto.DepartmentID) ?? "Unassigned";
            return dto;
        }
    }
}
