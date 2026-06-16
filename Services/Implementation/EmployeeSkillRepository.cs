using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Domain.Enums;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    public class EmployeeSkillRepository : IEmployeeSkillRepository
    {
        private readonly ApplicationDbContext db;

        public EmployeeSkillRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<EmployeeSkill> AddEmployeeSkillAsync(EmployeeSkill skill)
        {
            if (skill == null) return null;

            if (!Enum.IsDefined(typeof(ProficiencyLevel), skill.ProficiencyLevel))
                throw new Exception("Invalid Proficiency Level");

            

                await db.EmployeeSkills.AddAsync(skill);
                await db.SaveChangesAsync();

                return skill;
            
        }
    }

}
