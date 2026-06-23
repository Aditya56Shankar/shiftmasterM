using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Repositories
{
    public interface ISkillRepository
    {
        Task<List<string>> GetEmployeeSkillsAsync(int userId);
    }
}
