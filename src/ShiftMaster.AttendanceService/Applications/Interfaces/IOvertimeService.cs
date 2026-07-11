using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.AttendanceService.Applications.DTOs;

namespace ShiftMaster.AttendanceService.Applications.Interfaces
{
    public interface IOvertimeService
    {
        Task<List<OvertimeAuthorisationDto>> GetPendingOvertimeAsync(int locationId);
        Task<OvertimeAuthorisationResponseDto> LogOvertimeAsync(CreateOvertimeDto dto);
        Task<OvertimeAuthorisationResponseDto> AuthoriseOvertimeAsync(int otId, int authorisedById, bool approved);
    }
}
