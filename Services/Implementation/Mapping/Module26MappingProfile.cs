using AutoMapper;
using Services.DTOs;
using shiftmaster.models;
using Domain.Enums;
namespace Services.Implementation.Mapping
{
	public class Module26MappingProfile : Profile
	{
		public Module26MappingProfile()
		{
			CreateMap<CoverAssignment, CoverAssignmentResponseDto>();
			CreateMap<SwapRequest, SwapRequestResponseDto>();
			CreateMap<OvertimeAuthorisation, OvertimeAuthorisationResponseDto>();
			CreateMap<CreateCoverAssignmentDto, CoverAssignment>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CoverStatus.Assigned));
			CreateMap<CreateOvertimeDto,OvertimeAuthorisation>()
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApprovalStatus.Pending));
			CreateMap<CreateSwapRequestDto,SwapRequest>()
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApprovalStatus.Pending));
		}
	}
}