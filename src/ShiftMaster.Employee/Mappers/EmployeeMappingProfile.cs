using AutoMapper;
using ShiftMaster.Employee.Models;
using ShiftMaster.Employee.DTOs;
using ShiftMaster.Employee.Enums;
using System;

namespace ShiftMaster.Employee.Mappers
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            // Availability Mappings
            CreateMap<AvailabilityRequestDto, AvailabilitySubmission>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AvailabilityStatus.Submitted))
                .ForMember(dest => dest.SubmittedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<AvailabilitySubmission, AvailabilityResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<AvailabilitySubmission, AvailabilityDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Leave Mappings
            CreateMap<LeaveBlockRequestDto, LeaveBlock>()
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => Enum.Parse<LeaveReason>(src.Reason, true)));

            CreateMap<LeaveBlock, LeaveBlockResponseDto>()
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Employee Skill Mappings
            CreateMap<EmployeeSkillRequestDto, EmployeeSkill>()
                .ForMember(dest => dest.ProficiencyLevel, opt => opt.MapFrom(src => Enum.Parse<ProficiencyLevel>(src.ProficiencyLevel, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ActiveStatus.Active));

            CreateMap<EmployeeSkill, EmployeeSkillResponseDto>()
                .ForMember(dest => dest.ProficiencyLevel, opt => opt.MapFrom(src => src.ProficiencyLevel.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<EmployeeSkill, EmployeeSkillDto>()
                .ForMember(dest => dest.ProficiencyLevel, opt => opt.MapFrom(src => src.ProficiencyLevel.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // SkillRequirement Mappings
            CreateMap<SkillRequirement, SkillRequirementDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateSkillRequirementDto, SkillRequirement>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));

            CreateMap<UpdateSkillRequirementDto, SkillRequirement>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));
        }
    }
}
