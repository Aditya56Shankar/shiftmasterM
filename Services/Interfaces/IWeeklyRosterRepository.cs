using System;
using System.Collections.Generic;
using System.Text;
using Services.DTOs;
using shiftmaster.models;


namespace Services.Interfaces
{
    public interface IWeeklyRosterRepository
    {
        Task<WeeklyRoster> AddAsync(WeeklyRoster roster);
        Task<SupervisorRosterResponseDto?> GetRosterAsync(int locationId, DateTime weekStartDate);
    }
}

