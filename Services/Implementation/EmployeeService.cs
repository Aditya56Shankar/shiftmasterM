using AutoMapper;
using Services.DTOs;
using Services.Interfaces;
using Services.Interfaces.Repositories;

using Services.DTOs;
using System;



namespace Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeFullDto>> GetEmployeesFullData(int locationId, DateTime date)
        {
            var employees = await _employeeRepository.GetEmployeesWithFullDetails(locationId);

            var dayName = date.DayOfWeek.ToString();

            var filteredEmployees = employees
                .Select(e =>
                {
                    e.Availabilities = e.Availabilities
                        .Where(a =>
                            a.WeekStartDate <= date &&
                            a.WeekStartDate.AddDays(6) >= date &&
                            a.AvailableDays.Contains(dayName)
                        ).ToList();

                    return e;
                })
                .Where(e => e.Availabilities.Any())
                .ToList();

            return _mapper.Map<List<EmployeeFullDto>>(filteredEmployees);
        }
    }
}