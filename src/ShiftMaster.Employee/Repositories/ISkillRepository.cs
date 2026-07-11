using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftMaster.Employee.Repositories
{
    public interface ISkillRepository
    {
        Task<List<string>> GetEmployeeSkillsAsync(int userId);
    }
}
