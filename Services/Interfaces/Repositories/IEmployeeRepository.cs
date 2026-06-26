using System;
using System.Collections.Generic;
using System.Text;
using ShiftMaster.models;

namespace Services.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<User>> GetEmployeesWithFullDetails(int locationId);
    }
}
