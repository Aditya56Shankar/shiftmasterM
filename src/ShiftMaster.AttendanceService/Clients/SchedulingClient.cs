using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftMaster.AttendanceService.Clients
{
    public class SchedulingClient : ISchedulingClient
    {
        private readonly HttpClient _httpClient;

        public SchedulingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmployeeShiftShortDto?> GetAssignmentAsync(int assignmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/shiftassignment/internal/assignment/{assignmentId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<EmployeeShiftShortDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
