using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Application.DTOs;

namespace ShiftMaster.Employee.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeFullDto>> GetEmployeesFullData(int locationId, DateTime date);
    }
}
