using System;
using System.Collections.Generic;
using System.Text;
using ShiftMaster.models;

namespace Services.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User> GetUserByEmailWithRoleAsync(string email);
        Task AddUserAsync(User user);
        Task<int?> GetUserIdByEmailAsync(string email);
        Task<User> GetUserWithDetailsByIdAsync(int id);
        Task<bool> EmployeeIdExistsAsync(string employeeId);
        Task UpdateUserAsync(User user);
    }
}
