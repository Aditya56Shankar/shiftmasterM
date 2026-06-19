using Services.DTOs;

namespace Services.Interfaces
{
	public interface IOvertimeService
	{
		Task<List<OvertimeAuthorisationDto>> GetPendingOvertimeAsync(int locationId);
		Task<OvertimeAuthorisationResponseDto> LogOvertimeAsync(CreateOvertimeDto dto);
		Task<OvertimeAuthorisationResponseDto> AuthoriseOvertimeAsync(int otId, int authorisedById, bool approved);
	}
}
