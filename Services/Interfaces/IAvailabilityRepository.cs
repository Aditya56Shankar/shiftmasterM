using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces
{
    public interface IAvailabilityRepository
    {
        Task<AvailabilitySubmission> AddAvailableAsync(AvailabilitySubmission avail);
    }
}
