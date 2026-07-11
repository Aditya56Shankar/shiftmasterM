using AutoMapper;
using System;
using ShiftMaster.IdentityService.Application.DTOs;
using ShiftMaster.IdentityService.Domain.Models;
using ShiftMaster.IdentityService.Domain.Enums;

namespace ShiftMaster.IdentityService.Application.Mappers
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            // Department Mappings
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.departmentId))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.departmentName))
                .ReverseMap();

            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.departmentName, opt => opt.MapFrom(src => src.DepartmentName));

            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.departmentName, opt => opt.MapFrom(src => src.DepartmentName));

            // Role Mappings
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.roleId))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.roleName))
                .ReverseMap();

            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.roleName, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.roleName, opt => opt.MapFrom(src => src.RoleName));

            // WorkLocation Mappings
            CreateMap<WorkLocation, WorkLocationDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();

            CreateMap<CreateWorkLocationDto, WorkLocation>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<LocationType>(src.Type, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));

            CreateMap<UpdateWorkLocationDto, WorkLocation>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<LocationType>(src.Type, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));

            // User Mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.HomeLocation != null ? src.HomeLocation.LocationName : "Unassigned"))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.departmentName : "Unassigned"));

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UserStatus>(src.Status, true)))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UserStatus>(src.Status, true)));
        }
    }
}
