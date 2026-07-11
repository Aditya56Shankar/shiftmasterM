using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.AttendanceService.Data;
using ShiftMaster.AttendanceService.Enums;
using ShiftMaster.AttendanceService.Models;

namespace ShiftMaster.AttendanceService.Repositories
{
    public class OvertimeRepository : IOvertimeRepository
    {
        private readonly AttendanceDbContext _context;

        public OvertimeRepository(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<List<OvertimeAuthorisation>> GetPendingOvertimeByUserIdsAsync(List<int> userIds)
        {
            return await _context.OvertimeAuthorisations
                .Where(oa => oa.Status == ApprovalStatus.Pending && userIds.Contains(oa.UserID))
                .ToListAsync();
        }

        public async Task<OvertimeAuthorisation> AddOvertimeAsync(OvertimeAuthorisation overtimeAuthorisation)
        {
            await _context.OvertimeAuthorisations.AddAsync(overtimeAuthorisation);
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
