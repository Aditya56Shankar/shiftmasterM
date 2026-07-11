using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Application.DTOs;
using ShiftMaster.IdentityService.Application.Interfaces;
using ShiftMaster.IdentityService.Domain.Enums;
using ShiftMaster.IdentityService.Domain.Models;
using ShiftMaster.IdentityService.Infrastructure.Data;

namespace ShiftMaster.IdentityService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityDbContext _context;

        public UserService(IdentityDbContext context)
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
            user.Status = Enum.Parse<UserStatus>(dto.Status, true);
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
                Status = Enum.Parse<UserStatus>(newUser.Status, true),
                LocationID = newUser.LocationID,
                DepartmentID = newUser.DepartmentID,
                RoleID = newUser.RoleID
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GetUserByIdAsync(user.UserID) ?? new UserDto { UserID = user.UserID, Name = user.Name };
        }
    }
}
