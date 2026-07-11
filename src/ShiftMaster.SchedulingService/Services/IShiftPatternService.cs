using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftMaster.SchedulingService.DTOs;

namespace ShiftMaster.SchedulingService.Services
{
    public interface IShiftPatternService
    {
        Task<IEnumerable<ShiftPatternDto>> GetAllPatternsAsync();
        Task<ShiftPatternDto?> GetPatternByIdAsync(int id);
        Task<ShiftPatternDto> CreatePatternAsync(CreateShiftPatternDto newPattern);
        Task<ShiftPatternDto?> UpdatePatternAsync(int id, CreateShiftPatternDto updatePattern);
        Task<bool> DeletePatternAsync(int id);
    }
}
