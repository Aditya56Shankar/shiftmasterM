using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.Employee.Models;
using ShiftMaster.Employee.DTOs;
using ShiftMaster.Employee.Enums;

namespace ShiftMaster.Employee.Services
{
    public interface IAvailabilityService
    {
        Task<EmployeeScheduleDto> GetMyScheduleAsync(int userId);
        Task<AvailabilitySubmission> AddAvailableAsync(AvailabilitySubmission avail);
        Task<bool> UpdateAvailabilityStatusAsync(int id, AvailabilityStatus status);
        Task<bool> IsConfirmedAsync(int userId, DateTime date);
        Task<AvailabilitySubmission?> GetAvailabilityAsync(int userId, DateTime targetDate);
    }
}
