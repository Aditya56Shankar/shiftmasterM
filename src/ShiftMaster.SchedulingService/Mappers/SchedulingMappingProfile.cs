using AutoMapper;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.DTOs;
using ShiftMaster.SchedulingService.Enums;
using System;

namespace ShiftMaster.SchedulingService.Mappers
{
    public class SchedulingMappingProfile : Profile
    {
        public SchedulingMappingProfile()
        {
            // Roster Mappings
            CreateMap<CreateRosterDto, WeeklyRoster>()
                .ForMember(dest => dest.WeekStartDate, opt => opt.MapFrom(src => src.WeekStartDate.Date))
                .ForMember(dest => dest.WeekEndDate, opt => opt.MapFrom(src => src.WeekStartDate.Date.AddDays(6)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RosterStatus.Draft))
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<WeeklyRoster, WeeklyRosterDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();

            CreateMap<WeeklyRoster, SupervisorRosterResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ShiftAssignments, opt => opt.Ignore())
                .ForMember(dest => dest.Violations, opt => opt.Ignore());

            // Shift Mappings
            CreateMap<CreateAssignmentDto, ShiftAssignment>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.Date))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ShiftAssignmentStatus.Assigned));

            CreateMap<ShiftAssignment, AssignmentResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ShiftAssignment, SupervisorAssignmentViewDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ShiftAssignment, ShiftAssignmentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PatternName, opt => opt.MapFrom(src => src.Pattern != null ? src.Pattern.PatternName : "None"));

            // Violations
            CreateMap<SchedulingConstraintViolation, SchedulingConstraintViolationDto>()
                .ForMember(dest => dest.ViolationType, opt => opt.MapFrom(src => src.ViolationType.ToString()))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<SchedulingConstraintViolation, ViolationViewDto>()
                .ForMember(dest => dest.ViolationType, opt => opt.MapFrom(src => src.ViolationType.ToString()))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // ShiftPattern Mappings
            CreateMap<ShiftPattern, ShiftPatternDto>()
                .ForMember(dest => dest.ShiftType, opt => opt.MapFrom(src => src.ShiftType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();

            CreateMap<CreateShiftPatternDto, ShiftPattern>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.EndTime)))
                .ForMember(dest => dest.ShiftType, opt => opt.MapFrom(src => Enum.Parse<ShiftType>(src.ShiftType, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));

            CreateMap<UpdateShiftPatternDto, ShiftPattern>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.EndTime)))
                .ForMember(dest => dest.ShiftType, opt => opt.MapFrom(src => Enum.Parse<ShiftType>(src.ShiftType, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));

            // SwapRequest Mappings
            CreateMap<CreateSwapRequestDto, SwapRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApprovalStatus.Pending));

            CreateMap<SwapRequest, SwapRequestResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // CoverAssignment Mappings
            CreateMap<CreateCoverAssignmentDto, CoverAssignment>()
                .ForMember(dest => dest.CoverType, opt => opt.MapFrom(src => Enum.Parse<CoverType>(src.CoverType, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CoverStatus.Assigned));

            CreateMap<CoverAssignment, CoverAssignmentResponseDto>()
                .ForMember(dest => dest.CoverType, opt => opt.MapFrom(src => src.CoverType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
