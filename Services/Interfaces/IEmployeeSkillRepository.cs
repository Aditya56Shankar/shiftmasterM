using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces
{
    public interface IEmployeeSkillRepository
    {
        Task<EmployeeSkill> AddEmployeeSkillAsync(EmployeeSkill skill);
    }
}
