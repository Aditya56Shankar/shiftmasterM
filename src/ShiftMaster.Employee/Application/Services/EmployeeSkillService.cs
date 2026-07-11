using System;
using System.Threading.Tasks;
using ShiftMaster.Employee.Application.Interfaces;
using ShiftMaster.Employee.Domain.Enums;
using ShiftMaster.Employee.Domain.Models;
using ShiftMaster.Employee.Infrastructure.Repositories;

namespace ShiftMaster.Employee.Application.Services
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
                throw new Exception("Employee skill data cannot be null.");

            if (!Enum.IsDefined(typeof(ProficiencyLevel), skill.ProficiencyLevel))
                throw new Exception($"Invalid proficiency level: {skill.ProficiencyLevel}.");

            if (skill.CertifiedDate.Date >= DateTime.UtcNow.Date)
            {
                throw new Exception(
                    $"Employee skill certification date ({skill.CertifiedDate:yyyy-MM-dd}) cannot be today or a future date.");
            }

            await repository.AddAsync(skill);
            await repository.SaveAsync();

            return skill;
        }
    }
}
