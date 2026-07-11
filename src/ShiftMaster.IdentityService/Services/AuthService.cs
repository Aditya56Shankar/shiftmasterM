using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShiftMaster.IdentityService.DTOs;
using ShiftMaster.IdentityService.Models;
using ShiftMaster.IdentityService.Enums;
using ShiftMaster.IdentityService.Repositories;

namespace ShiftMaster.IdentityService.Services
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
            if (await _authRepository.EmailExistsAsync(dto.Email))
                throw new Exception("Email is already registered.");
            if (await _authRepository.EmployeeIdExistsAsync(dto.EmployeeID))
                throw new Exception("The Employee id has already been registered. So login with that user credentials.");

            var passwordPattern = @"^(?=.*[a-zA-Z0-9])(?=.*[^a-zA-Z0-9]).{6,}$";
            if (!Regex.IsMatch(dto.Password, passwordPattern))
            {
                throw new Exception("Password doesn't meet requirements. It should consists of alphanumeric, symbols and it should be minimum length of 6 characters.");
            }

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

            await _authRepository.AddUserAsync(user);

            return "User registered successfully.";
        }

        public async Task<object> LoginAsync(LoginDto dto)
        {
            var user = await _authRepository.GetUserByEmailWithRoleAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password.");

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Valid for 7 days

            await _authRepository.UpdateUserAsync(user);

            return new { token, refreshToken };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "superSecretKeyShiftMaster123!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("role", user.Role?.roleName ?? "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<int?> GetUserIdByEmailAsync(string email)
        {
            return await _authRepository.GetUserIdByEmailAsync(email);
        }

        public async Task<AdminUserDto?> GetAdminUserByIdAsync(int id)
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
                LocationName = user.HomeLocation?.LocationName,
                RoleName = user.Role?.roleName,
                DepartmentName = user.Department?.departmentName
            };
        }
    }
}
