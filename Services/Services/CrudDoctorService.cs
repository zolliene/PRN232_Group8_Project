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

        public async Task<bool> CreateDoctorAsync(CreateDoctorDTO dto)
        {
            // Tạo user mới cho bác sĩ
            var user = new User
            {
                Username = dto.Name,
                Email = dto.Email,
                IsActive = true,
                Role = "Doctor"
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

  
            doctor.LicenseNumber = dto.LicenseNumber;
            doctor.Qualification = dto.Qualification;
            doctor.DateOfLicense = dto.DateOfLicense;
            doctor.LicenseExpiryDate = dto.LicenseExpiryDate;

            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
                return false;

            _unitOfWork.DoctorRepository.Remove(doctor);
            await _unitOfWork.SaveAsync();
            return true;
        }

     
    }
}
