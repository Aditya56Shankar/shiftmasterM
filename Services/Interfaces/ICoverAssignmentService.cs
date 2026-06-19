using Services.DTOs;

namespace Services.Interfaces
{
	public interface ICoverAssignmentService
	{
		Task<List<CoverEligibilityDto>> GetEligibleCoversAsync(int shiftAssignmentId);
		Task<CoverAssignmentResponseDto> AssignCoverAsync(CreateCoverAssignmentDto dto);
	}
}
