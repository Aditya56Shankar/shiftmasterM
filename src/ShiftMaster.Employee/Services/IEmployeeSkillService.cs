using System.Threading.Tasks;
using ShiftMaster.Employee.Models;

namespace ShiftMaster.Employee.Services
{
    public interface IEmployeeSkillService
    {
        Task<EmployeeSkill> AddEmployeeSkillAsync(EmployeeSkill skill);
    }
}
