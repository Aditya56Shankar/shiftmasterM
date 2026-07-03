using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.models;
using Services.DTOs;
using Services.Interfaces;
using Domain.Repositories;

namespace Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
        {
            var r = await _repo.GetByIdAsync(roleId);
            return _mapper.Map<RoleDto>(r);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto newRole)
        {
            var role = _mapper.Map<Role>(newRole);
            await _repo.AddAsync(role);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto dto)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null) return null;

            _mapper.Map(dto, role);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null) return false;

            if (await _repo.HasLinkedUsersAsync(id))
                throw new InvalidOperationException("Cannot delete role while active profiles hold this access level.");

            _repo.Remove(role);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}