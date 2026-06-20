using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Enums; // Adjust namespace to match your project
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models; // Adjust to match your ShiftPattern domain model namespace

namespace Services.Implementation
{
    public class ShiftPatternService : IShiftPatternService
    {
        private readonly ApplicationDbContext _context;

        public ShiftPatternService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShiftPatternDto>> GetAllPatternsAsync()
        {
            return await _context.ShiftPatterns
                .Select(p => new ShiftPatternDto
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
                }).ToListAsync();
        }

        public async Task<ShiftPatternDto?> GetPatternByIdAsync(int id)
        {
            var p = await _context.ShiftPatterns.FindAsync(id);
            if (p == null) return null;

            return new ShiftPatternDto
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

            _context.ShiftPatterns.Add(pattern);
            await _context.SaveChangesAsync();

            return await GetPatternByIdAsync(pattern.PatternID) ?? new ShiftPatternDto();
        }

        public async Task<ShiftPatternDto?> UpdatePatternAsync(int id, CreateShiftPatternDto updatePattern)
        {
            var pattern = await _context.ShiftPatterns.FindAsync(id);
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

            await _context.SaveChangesAsync();
            return await GetPatternByIdAsync(id);
        }

        public async Task<bool> DeletePatternAsync(int id)
        {
            var pattern = await _context.ShiftPatterns.FindAsync(id);
            if (pattern == null) return false;

            // Protection Check: Ensure we don't sever historical roster mappings
            var isLinked = await _context.ShiftAssignments.AnyAsync(sa => sa.ShiftPatternID == id);
            if (isLinked) throw new InvalidOperationException("Cannot delete this template because active shift allocations are scheduled against it.");

            _context.ShiftPatterns.Remove(pattern);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}