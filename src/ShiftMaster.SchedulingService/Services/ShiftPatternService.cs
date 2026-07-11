using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShiftMaster.SchedulingService.Enums;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Repositories;
using ShiftMaster.SchedulingService.DTOs;

namespace ShiftMaster.SchedulingService.Services
{
    public class ShiftPatternService : IShiftPatternService
    {
        private readonly IShiftPatternRepository _repo;
        private readonly IMapper _mapper;

        public ShiftPatternService(IShiftPatternRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShiftPatternDto>> GetAllPatternsAsync()
        {
            var patterns = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ShiftPatternDto>>(patterns);
        }

        public async Task<ShiftPatternDto?> GetPatternByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            return p == null ? null : MapToDto(p);
        }

        public async Task<ShiftPatternDto> CreatePatternAsync(CreateShiftPatternDto newPattern)
        {
            var pattern = new ShiftPattern
            {
                PatternName = newPattern.PatternName,
                StartTime = TimeSpan.Parse(newPattern.StartTime),
                EndTime = TimeSpan.Parse(newPattern.EndTime),
                DurationHours = newPattern.DurationHours,
                BreakMinutes = newPattern.BreakMinutes,
                ShiftType = Enum.Parse<ShiftType>(newPattern.ShiftType, true),
                MinStaffingLevel = newPattern.MinStaffingLevel,
                Status = Enum.Parse<ActiveStatus>(newPattern.Status, true),
                LocationID = newPattern.LocationID
            };

            await _repo.AddAsync(pattern);
            await _repo.SaveChangesAsync();

            return MapToDto(pattern);
        }

        public async Task<ShiftPatternDto?> UpdatePatternAsync(int id, CreateShiftPatternDto updatePattern)
        {
            var pattern = await _repo.GetByIdAsync(id);
            if (pattern == null) return null;

            pattern.PatternName = updatePattern.PatternName;
            pattern.StartTime = TimeSpan.Parse(updatePattern.StartTime);
            pattern.EndTime = TimeSpan.Parse(updatePattern.EndTime);
            pattern.DurationHours = updatePattern.DurationHours;
            pattern.BreakMinutes = updatePattern.BreakMinutes;
            pattern.ShiftType = Enum.Parse<ShiftType>(updatePattern.ShiftType, true);
            pattern.MinStaffingLevel = updatePattern.MinStaffingLevel;
            pattern.Status = Enum.Parse<ActiveStatus>(updatePattern.Status, true);
            pattern.LocationID = updatePattern.LocationID;

            await _repo.SaveChangesAsync();
            return MapToDto(pattern);
        }

        public async Task<bool> DeletePatternAsync(int id)
        {
            var pattern = await _repo.GetByIdAsync(id);
            if (pattern == null) return false;

            if (await _repo.HasLinkedAssignmentsAsync(id))
                throw new InvalidOperationException("Cannot delete this template because active shift allocations are scheduled against it.");

            _repo.Remove(pattern);
            await _repo.SaveChangesAsync();
            return true;
        }

        private static ShiftPatternDto MapToDto(ShiftPattern p) => new()
        {
            PatternID = p.PatternID,
            PatternName = p.PatternName,
            StartTime = p.StartTime,
            EndTime = p.EndTime,
            DurationHours = p.DurationHours,
            BreakMinutes = p.BreakMinutes,
            ShiftType = p.ShiftType.ToString(),
            MinStaffingLevel = p.MinStaffingLevel,
            Status = p.Status.ToString(),
            LocationID = p.LocationID
        };
    }
}
