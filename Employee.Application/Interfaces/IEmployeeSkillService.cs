using System.Threading.Tasks;
using ShiftMaster.Employee.Domain.Models;

namespace ShiftMaster.Employee.Application.Interfaces
{
    public interface IEmployeeSkillService
    {
        Task<EmployeeSkill> AddEmployeeSkillAsync(EmployeeSkill skill);
    }
}
