using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.Employee.Services;

namespace ShiftMaster.Employee.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("location/{locationId}/full")]
        public async Task<IActionResult> GetEmployeesFullData(int locationId, [FromQuery] DateTime date)
        {
            var result = await _employeeService.GetEmployeesFullData(locationId, date);
            return Ok(result);
        }
    }
}
