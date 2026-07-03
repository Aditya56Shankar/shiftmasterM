using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly ApplicationDbContext _context;

    public SkillRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetEmployeeSkillsAsync(int userId)
    {
        return await _context.EmployeeSkills
            .Where(es =>
                es.UserID == userId &&
                es.Status == ActiveStatus.Active)
            .Select(es => es.SkillName)
            .ToListAsync();
    }
}