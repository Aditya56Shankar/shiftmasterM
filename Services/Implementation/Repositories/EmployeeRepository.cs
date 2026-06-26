using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Repositories;
using ShiftMaster.models;

namespace Services.Implementation.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetEmployeesWithFullDetails(int locationId)
        {
            return await _context.Users
                .Where(u => u.LocationID == locationId)
                .Include(u => u.Availabilities)
                .Include(u => u.Skills)
                .ToListAsync();
        }
    }

}
