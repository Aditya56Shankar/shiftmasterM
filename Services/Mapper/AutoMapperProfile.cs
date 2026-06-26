using System;
using AutoMapper;
using Domain.Enums;
using Domain.models;
using Services.DTOs;
using shiftmaster.models;
using ShiftMaster.models;

namespace Services.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Roster Mappings
            CreateMap<CreateRosterDto, WeeklyRoster>()
                .ForMember(dest => dest.WeekStartDate, opt => opt.MapFrom(src => src.WeekStartDate.Date))
                .ForMember(dest => dest.WeekEndDate, opt => opt.MapFrom(src => src.WeekStartDate.Date.AddDays(6)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RosterStatus.Draft))
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<WeeklyRoster, RosterResponseDto>();

            CreateMap<WeeklyRoster, SupervisorRosterResponseDto>()
                .ForMember(dest => dest.ShiftAssignments, opt => opt.Ignore())
                .ForMember(dest => dest.Violations, opt => opt.Ignore());

            // Shift Assignment Mappings
            CreateMap<ShiftAssignment, SupervisorAssignmentViewDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateAssignmentDto, ShiftAssignment>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.Date))
                    .ForMember(dest => dest.Status, opt => opt.Ignore()); // ✅ PREVENT DEFAULT BUG
            
            CreateMap<SchedulingConstraintViolation, ViolationViewDto>()
                .ForMember(dest => dest.ViolationType, opt => opt.MapFrom(src => src.ViolationType.ToString()))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Availability Mappings
            CreateMap<AvailabilityRequestDto, AvailabilitySubmission>();
            CreateMap<AvailabilitySubmission, AvailabilityResponseDto>();

            // Leave Mappings
            CreateMap<LeaveBlockRequestDto, LeaveBlock>();
            CreateMap<LeaveBlock, LeaveBlockResponseDto>()
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Employee Skill Mappings
            CreateMap<EmployeeSkillRequestDto, EmployeeSkill>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ActiveStatus.Active));

            CreateMap<EmployeeSkill, EmployeeSkillResponseDto>()
                .ForMember(dest => dest.ProficiencyLevel, opt => opt.MapFrom(src => src.ProficiencyLevel.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Attendance Mappings
            CreateMap<CreateAttendanceDto, AttendanceRecord>();

            CreateMap<AttendanceRecord, AttendanceDtoResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Timesheet Mappings
            CreateMap<TimesheetSummary, TimesheetDtoResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

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

            // Notification Mappings
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Name : "Unknown Employee"))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<NotificationCategory>(src.Category, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => NotificationStatus.Unread))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ShiftPattern Mappings
            CreateMap<ShiftPattern, ShiftPatternDto>().ReverseMap();
            CreateMap<CreateShiftPatternDto, ShiftPattern>();

            // SkillRequirement Mappings
            CreateMap<SkillRequirement, SkillRequirementDto>()
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationName : "Unassigned"))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.departmentName : "Unassigned"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<AttendanceRecord, AttendanceDtoResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString())); 

            CreateMap<CreateSkillRequirementDto, SkillRequirement>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));

            CreateMap<TimesheetSummary, TimesheetDtoResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString())); 
            CreateMap<UpdateSkillRequirementDto, SkillRequirement>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ActiveStatus>(src.Status, true)));

            // WorkLocation Mappings
            CreateMap<WorkLocation, WorkLocationDto>().ReverseMap();
            CreateMap<UpdateWorkLocationDto, WorkLocation>();

            // employe by id and date
            // ✅ Employee full mapping
            CreateMap<User, EmployeeFullDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.Availabilities))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills));

            // ✅ Availability mapping
            CreateMap<AvailabilitySubmission, AvailabilityDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // ✅ Skill mapping
            CreateMap<EmployeeSkill, EmployeeSkillDto>()
                .ForMember(dest => dest.ProficiencyLevel, opt => opt.MapFrom(src => src.ProficiencyLevel.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        }
    }
}