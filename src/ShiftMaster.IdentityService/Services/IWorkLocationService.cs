using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.IdentityService.DTOs;

namespace ShiftMaster.IdentityService.Services
{
    public interface IWorkLocationService
    {
        Task<IEnumerable<WorkLocationDto>> GetAllLocationsAsync();
        Task<WorkLocationDto?> GetLocationByIdAsync(int locationId);
        Task<WorkLocationDto> CreateLocationAsync(CreateWorkLocationDto newLocation);
        Task<WorkLocationDto?> UpdateLocationAsync(int id, UpdateWorkLocationDto dto);
        Task<bool> DeleteLocationAsync(int id);
        Task<bool> UpdateLocationStatusAsync(int locationId, string status);
    }
}
