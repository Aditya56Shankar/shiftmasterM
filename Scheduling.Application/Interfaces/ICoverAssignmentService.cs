using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Application.DTOs;

namespace ShiftMaster.SchedulingService.Application.Interfaces
{
    public interface ICoverAssignmentService
    {
        Task<List<CoverEligibilityDto>> GetEligibleCoversAsync(int shiftAssignmentId);
        Task<CoverAssignmentResponseDto> AssignCoverAsync(CreateCoverAssignmentDto dto);
        Task<CoverAssignmentResponseDto> ConfirmCoverAsync(int coverId, int actorUserId);
    }
}
