using System.Threading.Tasks;
using ShiftMaster.Employee.Domain.Models;

namespace ShiftMaster.Employee.Infrastructure.Repositories
{
    public interface IEmployeeSkillRepository
    {
        Task<EmployeeSkill> AddAsync(EmployeeSkill skill);
        Task SaveAsync();
    }
}
