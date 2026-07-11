using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShiftMaster.AttendanceService.Clients
{
    public class EmployeeClient : IEmployeeClient
    {
        private readonly HttpClient _httpClient;

        public EmployeeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> HasActiveLeaveOnDateAsync(int userId, DateTime date)
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
    }
}
