using AutoMapper;
using Domain.Enums;
using Services.DTOs;
using shiftmaster.models;

namespace Services.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<CreateRosterDto, WeeklyRoster>()
                           .ForMember(dest => dest.WeekStartDate,
                               opt => opt.MapFrom(src => src.WeekStartDate.Date))
                           .ForMember(dest => dest.WeekEndDate,
                               opt => opt.MapFrom(src => src.WeekStartDate.Date.AddDays(6)))
                           .ForMember(dest => dest.Status,
                               opt => opt.MapFrom(src => RosterStatus.Draft))
                           .ForMember(dest => dest.PublishedDate,
                                opt => opt.MapFrom(src => DateTime.UtcNow));


            CreateMap<WeeklyRoster, RosterResponseDto>();




            CreateMap<ShiftAssignment, SupervisorAssignmentViewDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.Ignore()) // set manually
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<SchedulingConstraintViolation, ViolationViewDto>()
                .ForMember(dest => dest.ViolationType, opt => opt.MapFrom(src => src.ViolationType.ToString()))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<WeeklyRoster, SupervisorRosterResponseDto>();

            //post available
            CreateMap<AvailabilityRequestDto, AvailabilitySubmission>();
            CreateMap<AvailabilitySubmission, AvailabilityResponseDto>();

            //post leave
            CreateMap<LeaveBlockRequestDto, LeaveBlock>();
            CreateMap<LeaveBlock, LeaveBlockResponseDto>()
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            //post employee skill
            CreateMap<EmployeeSkillRequestDto, EmployeeSkill>();

            CreateMap<EmployeeSkill, EmployeeSkillResponseDto>()
                .ForMember(dest => dest.ProficiencyLevel,
                    opt => opt.MapFrom(src => src.ProficiencyLevel.ToString()))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));





        }
    }
}