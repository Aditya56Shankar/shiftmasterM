using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Enums;
using Domain.models;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using shiftmaster.models;
using ShiftMaster.models;

namespace Services.Implementation
{
    public class WorkLocationService : IWorkLocationService
    {
        private readonly ApplicationDbContext _context;

        public WorkLocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkLocationDto>> GetAllLocationsAsync()
        {
            return await _context.WorkLocations
                .Select(l => new WorkLocationDto
                {
                    LocationID = l.LocationID,
                    LocationName = l.LocationName,
                    Type = l.Type.ToString(),
                    City = l.City,
                    ManagerID = l.ManagerID ?? 0,
                    OperatingHours = l.OperatingHours,
                    Status = l.Status.ToString()
                }).ToListAsync();
        }

        public async Task<WorkLocationDto?> GetLocationByIdAsync(int locationId)
        {
            var l = await _context.WorkLocations.FindAsync(locationId);
            if (l == null) return null;

            return new WorkLocationDto
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                Type = l.Type.ToString(),
                City = l.City,
                ManagerID = l.ManagerID ?? 0,
                OperatingHours = l.OperatingHours,
                Status = l.Status.ToString()
            };
        }

        public async Task<WorkLocationDto> CreateLocationAsync(CreateWorkLocationDto newLocation)
        {
            var location = new WorkLocation
            {
                LocationName = newLocation.LocationName,
                Type = Enum.Parse<Domain.Enums.LocationType>(newLocation.Type, true),
                City = newLocation.City,
                // Since your DB column is nullable, passing null breaks the circular loop!
                ManagerID = null,
                OperatingHours = newLocation.OperatingHours,
                Status = Domain.Enums.ActiveStatus.Active
            };

            _context.WorkLocations.Add(location);
            await _context.SaveChangesAsync();

            return await GetLocationByIdAsync(location.LocationID);
        }

        public async Task<bool> UpdateLocationStatusAsync(int locationId, string status)
        {
            var location = await _context.WorkLocations.FindAsync(locationId);
            if (location == null) return false;

            location.Status = Enum.Parse<ActiveStatus>(status, true);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}