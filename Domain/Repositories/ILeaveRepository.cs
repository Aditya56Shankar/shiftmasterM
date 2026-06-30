using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Domain.Repositories
{
    public interface ILeaveRepository
    {
        Task<List<LeaveBlock>> GetActiveLeavesAsync(int userId);
    }

}
