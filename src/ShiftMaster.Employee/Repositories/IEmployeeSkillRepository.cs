using System.Threading.Tasks;
using ShiftMaster.Employee.Models;

namespace ShiftMaster.Employee.Repositories
{
    public interface IEmployeeSkillRepository
    {
        Task<EmployeeSkill> AddAsync(EmployeeSkill skill);
        Task SaveAsync();
    }
}
