using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.Employee.Data;
using ShiftMaster.Employee.Enums;

namespace ShiftMaster.Employee.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly EmployeeDbContext _context;

        public SkillRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetEmployeeSkillsAsync(int userId)
        {
            return await _context.EmployeeSkills
                .Where(es => es.UserID == userId && es.Status == ActiveStatus.Active)
                .Select(es => es.SkillName)
                .ToListAsync();
        }
    }
}
