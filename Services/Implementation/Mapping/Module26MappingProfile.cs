using AutoMapper;
using Services.DTOs;
using shiftmaster.models;

namespace Services.Implementation.Mapping
{
	public class Module26MappingProfile : Profile
	{
		public Module26MappingProfile()
		{
			CreateMap<CoverAssignment, CoverAssignmentResponseDto>();
			CreateMap<SwapRequest, SwapRequestResponseDto>();
			CreateMap<OvertimeAuthorisation, OvertimeAuthorisationResponseDto>();
		}
	}
}