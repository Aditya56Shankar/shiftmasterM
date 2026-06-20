using shiftmaster.models;

namespace Services.Interfaces.Repositories
{
	public interface IOvertimeRepository
	{
		Task<List<OvertimeAuthorisation>> GetPendingOvertimeByLocationAsync(int locationId);
		Task<OvertimeAuthorisation> AddOvertimeAsync(OvertimeAuthorisation overtimeAuthorisation);
		Task<OvertimeAuthorisation?> GetOvertimeByIdAsync(int otId);
		Task SaveChangesAsync();
	}
}
