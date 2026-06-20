using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Enums;
using Domain.models;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;
using ShiftMaster.models;

namespace Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.HomeLocation)
                .Include(u => u.Department)
                .Select(u => new UserDto
                {
                    UserID = u.UserID,
                    EmployeeID = u.EmployeeID,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Status = u.Status.ToString(),
                    LocationID = u.LocationID,
                    LocationName = u.HomeLocation != null ? u.HomeLocation.LocationName : "Unassigned",
                    DepartmentID = u.DepartmentID,
                    DepartmentName = u.Department != null ? u.Department.departmentName : "Unassigned",
                    RoleID = u.RoleID
                }).ToListAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var u = await _context.Users
                .Include(u => u.HomeLocation)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(user => user.UserID == userId);

            if (u == null) return null;

            return new UserDto
            {
                UserID = u.UserID,
                EmployeeID = u.EmployeeID,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Status = u.Status.ToString(),
                LocationID = u.LocationID,
                LocationName = u.HomeLocation != null ? u.HomeLocation.LocationName : "Unassigned",
                DepartmentID = u.DepartmentID,
                DepartmentName = u.Department != null ? u.Department.departmentName : "Unassigned",
                RoleID = u.RoleID
            };
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Status = Enum.Parse<Domain.Enums.UserStatus>(dto.Status, true);
            user.LocationID = dto.LocationID;
            user.DepartmentID = dto.DepartmentID;
            user.RoleID = dto.RoleID;

            await _context.SaveChangesAsync();
            return new UserDto { UserID = user.UserID, Name = user.Name, Email = user.Email };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto newUser)
        {
            var user = new User
            {
                EmployeeID = newUser.EmployeeID,
                Name = newUser.Name,
                Email = newUser.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
                Phone = newUser.Phone,
                Status = Enum.Parse<Domain.Enums.UserStatus>(newUser.Status, true),
                LocationID = newUser.LocationID,
                DepartmentID = newUser.DepartmentID,
                RoleID = newUser.RoleID // <-- Respecting the frontend selected Role ID directly now!
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GetUserByIdAsync(user.UserID) ?? new UserDto { UserID = user.UserID, Name = user.Name };
        }
    }
}