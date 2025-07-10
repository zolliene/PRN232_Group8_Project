using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto.request;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AuthService: IAuthService
    {
        private readonly  IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public AuthResult? Authenticate(string email, string password)
        {
            var user = _unitOfWork.UserRepository
                .FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null) return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResult { Id = user.Id, Token = tokenString, Role = user.Role };
        }
        public async Task RegisterPatient(RegisterPatientReq request)
        {
            // 1. Kiểm tra username/email đã tồn tại chưa
            bool exists = await _unitOfWork.UserRepository.ExistsAsync(u =>
                u.Username == request.Username || u.Email == request.Email);

            if (exists)
                throw new ArgumentException("Username or email already exists");

            // 2. Tạo User
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password, // (Nên mã hóa)
                Role = "patient",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync(); // để có Id

            // 3. Tạo Patient
            var patient = new Patient
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = DateOnly.FromDateTime(request.Dob),
                Gender = request.Gender,
                Phone = request.Phone,
                Address = request.Address,
                InsuranceNumber = request.InsuranceNumber,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.PatientRepository.AddAsync(patient);
            await _unitOfWork.SaveAsync();
        }
    }

    public class AuthResult
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }

}
