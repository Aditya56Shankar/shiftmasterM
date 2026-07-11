using AutoMapper;
using ShiftMaster.CommsAuditService.Models;
using ShiftMaster.CommsAuditService.DTOs;
using ShiftMaster.CommsAuditService.Enums;
using System;

namespace ShiftMaster.CommsAuditService.Mappers
{
    public class CommsAuditMappingProfile : Profile
    {
        public CommsAuditMappingProfile()
        {
            // Notification Mappings
            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<NotificationCategory>(src.Category, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => NotificationStatus.Unread))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.EmployeeName, opt => opt.Ignore());

            // AuditLog Mappings
            CreateMap<AuditLog, AuditLogDto>()
                .ForMember(dest => dest.Actor, opt => opt.Ignore());
        }
    }
}
