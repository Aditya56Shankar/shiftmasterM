using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using shiftmaster.models;


namespace Services.Implementation
{
    public class EmployeeSkillService : IEmployeeSkillService
    {
        private readonly IEmployeeSkillRepository repository;

        public EmployeeSkillService(IEmployeeSkillRepository repository)
        {
            this.repository = repository;
        }

        public async Task<EmployeeSkill> AddEmployeeSkillAsync(EmployeeSkill skill)
        {
            if (skill == null)
                return null;

            // ✅ Business validation belongs here
            if (!Enum.IsDefined(typeof(ProficiencyLevel), skill.ProficiencyLevel))
                throw new Exception("Invalid Proficiency Level");

            await repository.AddAsync(skill);
            await repository.SaveAsync();

            return skill;
        }


    }
}
