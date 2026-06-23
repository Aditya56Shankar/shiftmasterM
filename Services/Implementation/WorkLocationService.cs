using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Enums;
using Domain.Interfaces;
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
        private readonly IWorkLocationRepository _repository;

        // Notice we inject the Repository here, NOT the DbContext!
        public WorkLocationService(IWorkLocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WorkLocationDto>> GetAllLocationsAsync()
        {
            var locations = await _repository.GetAllAsync();
            return locations.Select(l => new WorkLocationDto
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                Type = l.Type.ToString(),
                City = l.City,
                ManagerID = l.ManagerID ?? 0,
                OperatingHours = l.OperatingHours,
                Status = l.Status.ToString()
            });
        }

        public async Task<WorkLocationDto?> GetLocationByIdAsync(int locationId)
        {
            var l = await _repository.GetByIdAsync(locationId);
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
                ManagerID = null,
                OperatingHours = newLocation.OperatingHours,
                Status = Domain.Enums.ActiveStatus.Active
            };

            await _repository.AddAsync(location);
            await _repository.SaveChangesAsync();

            return await GetLocationByIdAsync(location.LocationID);
        }

        public async Task<WorkLocationDto?> UpdateLocationAsync(int id, UpdateWorkLocationDto dto)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location == null) return null;

            location.LocationName = dto.LocationName;
            location.Type = Enum.Parse<LocationType>(dto.Type, true);
            location.City = dto.City;
            location.OperatingHours = dto.OperatingHours;
            location.Status = Enum.Parse<ActiveStatus>(dto.Status, true);
            location.ManagerID = dto.ManagerID;

            _repository.Update(location);
            await _repository.SaveChangesAsync();

            // Note: Fixed your incomplete mapping from the original snippet here
            return await GetLocationByIdAsync(location.LocationID);
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location == null) return false;

            // Business logic/protection check stays in the service layer!
            var hasLinkedUsers = await _repository.HasLinkedUsersAsync(id);
            if (hasLinkedUsers) throw new InvalidOperationException("Cannot delete location because employees are currently assigned to it.");

            _repository.Remove(location);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateLocationStatusAsync(int locationId, string status)
        {
            var location = await _repository.GetByIdAsync(locationId);
            if (location == null) return false;

            location.Status = Enum.Parse<ActiveStatus>(status, true);
            _repository.Update(location);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}