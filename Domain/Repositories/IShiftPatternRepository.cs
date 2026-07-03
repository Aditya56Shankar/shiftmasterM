using System;
using System.Collections.Generic;
using System.Text;
using shiftmaster.models;

namespace Domain.Repositories
{
    public interface IShiftPatternRepository
    {
        Task<IEnumerable<ShiftPattern>> GetAllAsync();
        Task<ShiftPattern?> GetByIdAsync(int id);
        Task AddAsync(ShiftPattern pattern);
        Task<bool> HasLinkedAssignmentsAsync(int id);
        void Remove(ShiftPattern pattern);
        Task SaveChangesAsync();
    }
}
