using System.Threading.Tasks;
using ShiftMaster.Employee.Data;
using ShiftMaster.Employee.Models;

namespace ShiftMaster.Employee.Repositories
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
