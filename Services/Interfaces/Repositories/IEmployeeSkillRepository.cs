using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces.Repositories
{
    public interface IEmployeeSkillRepository
    {
        Task<EmployeeSkill> AddAsync(EmployeeSkill skill);
        Task SaveAsync();
    }
}
