using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftMaster.AttendanceService.Clients
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

        public async Task<bool> WorkLocationExistsAsync(int locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/worklocations/{locationId}/exists");
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

        public async Task<Dictionary<int, string>> LookupUserNamesAsync(List<int> userIds)
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
    }
}
