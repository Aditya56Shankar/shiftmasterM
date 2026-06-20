using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;

namespace Services.Interfaces
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