using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;

namespace Services.Interfaces
{
    public interface IEmployeeService
    {
        public  Task<List<EmployeeFullDto>> GetEmployeesFullData(int locationId, DateTime date);
            
            }
}