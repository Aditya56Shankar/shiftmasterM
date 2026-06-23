using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Services.Interfaces.Repositories
{
    public interface ILeaveRepository
    {
        Task<List<LeaveBlock>> GetActiveLeavesAsync(int userId);
    }

}
