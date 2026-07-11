using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftMaster.Employee.DTOs;

namespace ShiftMaster.Employee.Clients
{
    public class SchedulingClient : ISchedulingClient
    {
        private readonly HttpClient _httpClient;

        public SchedulingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EmployeeShiftDto>> GetShiftsByUserIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/shiftassignment/internal/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<EmployeeShiftDto>>() ?? new List<EmployeeShiftDto>();
                }
                return new List<EmployeeShiftDto>();
            }
            catch
            {
                return new List<EmployeeShiftDto>();
            }
        }
    }
}
