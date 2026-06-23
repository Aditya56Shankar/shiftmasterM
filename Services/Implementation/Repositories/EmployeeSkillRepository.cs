using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using shiftmaster.models;

namespace Services.Implementation.Repositories
{

    public class EmployeeSkillRepository : IEmployeeSkillRepository
    {
        private readonly ApplicationDbContext db;

        public EmployeeSkillRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<EmployeeSkill> AddAsync(EmployeeSkill skill)
        {
            await db.EmployeeSkills.AddAsync(skill);
            return skill;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }

}
