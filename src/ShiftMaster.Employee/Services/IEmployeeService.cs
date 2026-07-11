using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.DTOs;

namespace ShiftMaster.Employee.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeFullDto>> GetEmployeesFullData(int locationId, DateTime date);
    }
}
