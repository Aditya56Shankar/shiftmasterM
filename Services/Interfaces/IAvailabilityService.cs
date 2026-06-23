using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using shiftmaster.models;

namespace Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<AvailabilitySubmission> AddAvailableAsync(AvailabilitySubmission avail);
        Task<AvailabilitySubmission> CheckAndUpdateAvailabilityAsync(ShiftAssignment assignment);

        Task<bool> UpdateAvailabilityStatusAsync(int id, AvailabilityStatus status); 

    }

}
