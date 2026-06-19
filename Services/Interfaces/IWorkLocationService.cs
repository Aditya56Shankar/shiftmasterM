using Services.DTOs;

namespace Services.Interfaces
{
    public interface IWorkLocationService
    {
        // "Task" means this happens asynchronously, which is standard for database calls
        Task<IEnumerable<WorkLocationDto>> GetAllLocationsAsync();
        Task<WorkLocationDto?> GetLocationByIdAsync(int locationId);
        Task<WorkLocationDto> CreateLocationAsync(CreateWorkLocationDto newLocation);
        Task<bool> UpdateLocationStatusAsync(int locationId, string status);
    }
}