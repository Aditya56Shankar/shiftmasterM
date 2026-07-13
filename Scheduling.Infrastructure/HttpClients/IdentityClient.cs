using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftMaster.SchedulingService.Clients
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/{userId}/exists");
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

        public async Task<bool> LocationExistsAsync(int locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/worklocations/{locationId}/exists");

                if (response.IsSuccessStatusCode)
                {
                    // 2. Read the response body explicitly as a plain text string
                    string rawContent = await response.Content.ReadAsStringAsync();

                    // 3. Clean up any accidental whitespace/quotes and parse it safely
                    if (bool.TryParse(rawContent.Trim().ToLower(), out bool exists))
                    {
                        return exists;
                    }
                }
                return false;
            }
            catch
            {
                // Suppresses network drops or parsing hiccups safely as false
                return false;
            }
        }

        public async Task<bool> DepartmentExistsAsync(int departmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/departments/{departmentId}/exists");
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

        public async Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/lookup", userIds);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Dictionary<int, string>>() ?? new Dictionary<int, string>();
                }
                return new Dictionary<int, string>();
            }
            catch
            {
                return new Dictionary<int, string>();
            }
        }

        public async Task<string?> GetLocationNameAsync(int locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/worklocations/internal/{locationId}/name");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> GetDepartmentNameAsync(int departmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/departments/internal/{departmentId}/name");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<int>> GetUserIdsByLocationAsync(int locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/location/{locationId}/ids");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<int>>() ?? new List<int>();
                }
                return new List<int>();
            }
            catch
            {
                return new List<int>();
            }
        }

        public async Task<List<UserShortDto>> GetActiveUsersByLocationAndDeptAsync(int locationId, int departmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/active-by-location-and-dept?locationId={locationId}&departmentId={departmentId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<UserShortDto>>() ?? new List<UserShortDto>();
                }
                return new List<UserShortDto>();
            }
            catch
            {
                return new List<UserShortDto>();
            }
        }

        public async Task<List<UserShortDto>> GetActiveUsersByLocationAsync(int locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/active-by-location?locationId={locationId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<UserShortDto>>() ?? new List<UserShortDto>();
                }
                return new List<UserShortDto>();
            }
            catch
            {
                return new List<UserShortDto>();
            }
        }
    }
}
