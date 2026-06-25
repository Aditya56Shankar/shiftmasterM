using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.DTOs;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using ShiftMaster.models;

namespace Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            // Replaced _context check with repository method
            if (await _authRepository.EmailExistsAsync(dto.Email))
                throw new Exception("Email is already registered.");

            var user = new User
            {
                EmployeeID = dto.EmployeeID,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                LocationID = dto.LocationID,
                RoleID = dto.RoleID,
                DepartmentID = dto.DepartmentID,
                Status = UserStatus.Active,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            // Replaced _context.Add and SaveChangesAsync with repository method
            await _authRepository.AddUserAsync(user);

            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            // Replaced _context query with repository method
            var user = await _authRepository.GetUserByEmailWithRoleAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password.");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();  //responsible for creating and validating JWT tokens
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("role", user.Role?.roleName.ToString() ?? "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration["Jwt:Issuer"] ?? "ShiftMasterAPI",
                Audience = _configuration["Jwt:Audience"] ?? "ShiftMasterUsers",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) 
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<int?> GetUserIdByEmailAsync(string email)
        {
            return await _authRepository.GetUserIdByEmailAsync(email);
        }

        public async Task<AdminUserDto> GetAdminUserByIdAsync(int id)
        {
            var user = await _authRepository.GetUserWithDetailsByIdAsync(id);

            if (user == null)
                return null;

            return new AdminUserDto
            {
                UserId = user.UserID,
                EmployeeID = user.EmployeeID,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                // Using null-conditional operators (?.) just in case related entities are null
                LocationName = user.HomeLocation?.LocationName,
                RoleName = user.Role?.roleName.ToString(),
                DepartmentName = user.Department?.departmentName
            };
        }
    }
}