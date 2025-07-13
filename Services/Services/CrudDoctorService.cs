using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CrudDoctorService : ICrudDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CrudDoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DoctorDTO>> GetCrudDoctorAccountsAsync()
        {
            var doctors = await _unitOfWork.DoctorRepository.GetAllIncludeAsync(d => d.User);

            var result = doctors
                .Where(d => d.User != null)
                .Select(d => new DoctorDTO
                {
                    Id = d.Id, 
                    Name = d.User.Username,
                    FullName = d.User.FullName,
                    Email = d.User.Email,
                    LicenseNumber = d.LicenseNumber,
                    Qualification = d.Qualification,
                    DateOfLicense = d.DateOfLicense,
                    LicenseExpiryDate = d.LicenseExpiryDate,
                    IsActive = d.User.IsActive
                })
                .ToList();

            return result;
        }

        public async Task<DoctorDTO> GetCrudDoctorAccountByIdAsync(int id)
        {
            var doctors = await _unitOfWork.DoctorRepository.GetAllIncludeAsync(d => d.User);

            var doctor = doctors
                .Where(d => d.Id == id && d.User != null)
                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    Name = d.User.Username,
                    FullName = d.User.FullName,
                    Email = d.User.Email,
                    LicenseNumber = d.LicenseNumber,
                    Qualification = d.Qualification,
                    DateOfLicense = d.DateOfLicense,
                    LicenseExpiryDate = d.LicenseExpiryDate,
                    IsActive = d.User.IsActive
                })
                .FirstOrDefault();

            if (doctor == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy bác sĩ với ID = {id}");
            }

            return doctor;
        }





        //public async Task<int?> CreateUserAsync(CreateUserDTO dto)
        //{
        //    // Check username trùng
        //    var existing = await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Username == dto.Username);
        //    if (existing != null)
        //        return null; 

        //    // Hash password nếu cần (hoặc giữ plain text nếu chưa làm login)
        //    var user = new User
        //    {
        //        Username = dto.Username,
        //        Email = dto.Email,
        //        Password = dto.Password,
        //        Role = dto.Role,
        //        IsActive = true,
        //        CreatedAt = DateTime.Now
        //    };

        //    await _unitOfWork.UserRepository.AddAsync(user);
        //    await _unitOfWork.SaveAsync();

        //    return user.Id;
        //}
        public async Task<bool> CreateDoctorAsync(CreateDoctorDTO dto)
        {

            var password = "123456";
            var hashed = BCrypt.Net.BCrypt.HashPassword(password); // dùng thư viện BCrypt.Net
        
            // Tạo user mới cho bác sĩ
            var user = new User
            {
                Username = dto.Name,
                FullName = dto.FullName,
                Email = dto.Email,
                Password = hashed,
                IsActive = true,
                Role = "doctor"
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync(); // Cần Save để có user.Id

            // Tạo hồ sơ bác sĩ liên kết với user vừa tạo
            var doctor = new Doctor
            {
                UserId = user.Id,
                LicenseNumber = dto.LicenseNumber,
                Qualification = dto.Qualification,
                DateOfLicense = dto.DateOfLicense,
                LicenseExpiryDate = dto.LicenseExpiryDate
            };

            await _unitOfWork.DoctorRepository.AddAsync(doctor);
            await _unitOfWork.SaveAsync();

            return true;
        }


        public async Task<bool> UpdateDoctorAsync(int userId, UpdateDoctorDTO dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            var doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
                return false;

            user.Username = dto.Name;
            user.Email = dto.Email;
            user.FullName = dto.FullName;
            doctor.LicenseNumber = dto.LicenseNumber;
            doctor.Qualification = dto.Qualification;
            doctor.DateOfLicense = dto.DateOfLicense;
            doctor.LicenseExpiryDate = dto.LicenseExpiryDate;

            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<bool> DeleteDoctorAsync(int id)
        {
            // Lấy doctor kèm theo User
            var doctorList = await _unitOfWork.DoctorRepository.GetAllAsync(q =>
                q.Include(d => d.User).Where(d => d.Id == id));

            var doctor = doctorList.FirstOrDefault();
            if (doctor == null) return false;

            // Nếu có User thì xóa luôn
            if (doctor.User != null)
            {
                _unitOfWork.UserRepository.Remove(doctor.User);
            }

            // Xóa bác sĩ
            _unitOfWork.DoctorRepository.Remove(doctor);

            await _unitOfWork.SaveAsync();
            return true;
        }

       
    }
}
