using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface ISkillRepository
    {
        Task<List<string>> GetEmployeeSkillsAsync(int userId);
    }
}
