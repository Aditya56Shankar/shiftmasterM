using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Repositories;
using shiftmaster.models;

namespace Services.Implementation.Repositories
{
	public class OvertimeRepository : IOvertimeRepository
	{
		private readonly ApplicationDbContext _context;

		public OvertimeRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<OvertimeAuthorisation>> GetPendingOvertimeByLocationAsync(int locationId)
		{
			return await _context.OvertimeAuthorisations
				.Include(oa => oa.Employee)
				.Where(oa => oa.Status == ApprovalStatus.Pending && oa.Employee.LocationID == locationId)
				.ToListAsync();
		}

		public async Task<OvertimeAuthorisation> AddOvertimeAsync(OvertimeAuthorisation overtimeAuthorisation)
		{
			_context.OvertimeAuthorisations.Add(overtimeAuthorisation);
			await _context.SaveChangesAsync();
			return overtimeAuthorisation;
		}

		public async Task<OvertimeAuthorisation?> GetOvertimeByIdAsync(int otId)
		{
			return await _context.OvertimeAuthorisations
				.FirstOrDefaultAsync(oa => oa.OTID == otId);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
