using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Application.DTOs;

namespace ShiftMaster.SchedulingService.Clients
{
    public class EmployeeAvailabilityShortDto
    {
        public int AvailabilityID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string AvailableDays { get; set; } = null!;
        public decimal MaxHoursPerWeek { get; set; }
        public string Status { get; set; } = null!;
    }

    public interface IEmployeeClient
    {
        Task<bool> IsConfirmedAsync(int userId, DateTime date);
        Task<List<string>> GetEmployeeSkillsAsync(int userId);
        Task<bool> IsOnLeaveAsync(int userId, DateTime date);
        Task<List<string>> GetRequiredSkillNamesAsync(int locationId, int departmentId);
        Task<EmployeeAvailabilityShortDto?> GetAvailabilityAsync(int userId, DateTime targetDate);
        Task<List<EmployeeFullDto>> GetEmployeesFullDataAsync(int locationId, DateTime date);
    }
}
