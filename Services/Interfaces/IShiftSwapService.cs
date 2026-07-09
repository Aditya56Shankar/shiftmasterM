using Services.DTOs;

namespace Services.Interfaces
{
	public interface IShiftSwapService
	{
		Task<List<SwapEligibilityDto>> GetEligibleSwapTargetsAsync(int shiftAssignmentId);
		Task<SwapRequestResponseDto> CreateSwapRequestAsync(CreateSwapRequestDto dto,int actorUserId);
		Task<SwapRequestResponseDto> RespondToSwapAsync(int swapId, bool accepted,int actorUserId);
		Task<SwapRequestResponseDto> ApproveSwapAsync(int swapId, int approvedById, bool approved);
    }
}
