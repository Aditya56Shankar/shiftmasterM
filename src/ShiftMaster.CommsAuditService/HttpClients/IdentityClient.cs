using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftMaster.CommsAuditService.Clients
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetUserNameAsync(int userId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/lookup", new List<int> { userId });
                if (response.IsSuccessStatusCode)
                {
                    var dict = await response.Content.ReadFromJsonAsync<Dictionary<int, string>>();
                    if (dict != null && dict.TryGetValue(userId, out var name))
                    {
                        return name;
                    }
                }
                return "Unknown Employee";
            }
            catch
            {
                return "Unknown Employee";
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
