using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.AttendanceService.DTOs;

namespace ShiftMaster.AttendanceService.Services
{
    public interface IOvertimeService
    {
        Task<List<OvertimeAuthorisationDto>> GetPendingOvertimeAsync(int locationId);
        Task<OvertimeAuthorisationResponseDto> LogOvertimeAsync(CreateOvertimeDto dto);
        Task<OvertimeAuthorisationResponseDto> AuthoriseOvertimeAsync(int otId, int authorisedById, bool approved);
    }
}
