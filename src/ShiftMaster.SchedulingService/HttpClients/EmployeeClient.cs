using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.Application.DTOs;

namespace ShiftMaster.SchedulingService.Clients
{
    public class EmployeeClient : IEmployeeClient
    {
        private readonly HttpClient _httpClient;

        public EmployeeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsConfirmedAsync(int userId, DateTime date)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/availability/internal/{userId}/availability/confirmed?date={date:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<string>> GetEmployeeSkillsAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/employeeskill/internal/{userId}/skills");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
                }
                return new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<bool> IsOnLeaveAsync(int userId, DateTime date)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/leave/internal/{userId}/active-on-date?date={date:yyyy-MM-dd}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<string>> GetRequiredSkillNamesAsync(int locationId, int departmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/skillrequirements/internal/skills?locationId={locationId}&departmentId={departmentId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
                }
                return new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<EmployeeAvailabilityShortDto?> GetAvailabilityAsync(int userId, DateTime targetDate)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/availability/internal/{userId}/availability?targetDate={targetDate:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<EmployeeAvailabilityShortDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<EmployeeFullDto>> GetEmployeesFullDataAsync(int locationId, DateTime date)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/employee/location/{locationId}/full?date={date:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<EmployeeFullDto>>() ?? new List<EmployeeFullDto>();
                }
                return new List<EmployeeFullDto>();
            }
            catch
            {
                return new List<EmployeeFullDto>();
            }
        }
    }
}
