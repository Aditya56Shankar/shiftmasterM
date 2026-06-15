using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;


namespace Services.Interfaces
{
    public interface IWeeklyRosterRepository
    {
        Task<WeeklyRoster> AddAsync(WeeklyRoster roster);
    }
}

