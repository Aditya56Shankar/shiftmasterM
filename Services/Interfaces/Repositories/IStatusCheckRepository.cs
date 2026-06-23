using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Repositories
{
    public interface IStatusCheckRepository
    {
        Task<bool> IsCoveredAsync(int userId);
        Task<bool> IsSwappedAsync(int userId);
        Task<bool> IsConfirmedAsync(int userId, DateTime date);
    }

}
