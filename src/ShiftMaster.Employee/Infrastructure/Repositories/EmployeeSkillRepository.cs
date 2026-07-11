using System.Threading.Tasks;
using ShiftMaster.Employee.Domain.Models;
using ShiftMaster.Employee.Infrastructure.Data;

namespace ShiftMaster.Employee.Infrastructure.Repositories
{
    public class EmployeeSkillRepository : IEmployeeSkillRepository
    {
        private readonly EmployeeDbContext db;

        public EmployeeSkillRepository(EmployeeDbContext db)
        {
            this.db = db;
        }

        public async Task<EmployeeSkill> AddAsync(EmployeeSkill skill)
        {
            await db.EmployeeSkills.AddAsync(skill);
            return skill;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
