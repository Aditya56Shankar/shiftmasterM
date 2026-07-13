using AutoMapper;
using System;
using ShiftMaster.AttendanceService.Domain.Models;
using ShiftMaster.AttendanceService.Domain.Enums;
using ShiftMaster.AttendanceService.Applications.DTOs;

namespace ShiftMaster.AttendanceService.Applications.Mappers
{
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile()
        {
            // Attendance Mappings
            CreateMap<CreateAttendanceDto, AttendanceRecord>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AttendanceStatus.Present));

            CreateMap<AttendanceRecord, AttendanceDtoResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Timesheet Mappings
            CreateMap<TimesheetSummary, TimesheetDtoResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Overtime Mappings
            CreateMap<CreateOvertimeDto, OvertimeAuthorisation>()
                .ForMember(dest => dest.OTType, opt => opt.MapFrom(src => Enum.Parse<OTType>(src.OTType, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApprovalStatus.Pending));

            CreateMap<OvertimeAuthorisation, OvertimeAuthorisationResponseDto>()
                .ForMember(dest => dest.OTType, opt => opt.MapFrom(src => src.OTType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<OvertimeAuthorisation, OvertimeAuthorisationDto>()
                .ForMember(dest => dest.OTType, opt => opt.MapFrom(src => src.OTType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.EmployeeName, opt => opt.Ignore());
        }
    }
}
