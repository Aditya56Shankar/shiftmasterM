using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.DTOs;

namespace ShiftMaster.SchedulingService.Services
{
    public interface ICoverAssignmentService
    {
        Task<List<CoverEligibilityDto>> GetEligibleCoversAsync(int shiftAssignmentId);
        Task<CoverAssignmentResponseDto> AssignCoverAsync(CreateCoverAssignmentDto dto);
        Task<CoverAssignmentResponseDto> ConfirmCoverAsync(int coverId, int actorUserId);
    }
}
