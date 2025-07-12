using AutoMapper;
using Repositories.UnitOfWork;
using Services.Dto.request;
using Services.Dto.response;
using Services.Interfaces;

namespace Services.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PatientProfileRes> GetProfile(int userId)
        {
            var patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(p => p.UserId == userId)
                          ?? throw new KeyNotFoundException("Patient not found");

            var user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == userId)
                       ?? throw new KeyNotFoundException("User not found");

            return new PatientProfileRes
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Dob = patient.Dob,
                Gender = patient.Gender,
                Phone = patient.Phone,
                Address = patient.Address,
                InsuranceNumber = patient.InsuranceNumber,
                Email = user.Email,
                Username = user.Username
            };
        }


        public async Task UpdateProfile(int userId, UpdatePatientProfileReq request)
        {
            var patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(p => p.UserId == userId)
                          ?? throw new KeyNotFoundException("Patient not found");

            var user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == userId)
                       ?? throw new KeyNotFoundException("User not found");

            // Update Patient
            patient.FirstName = request.FirstName;
            patient.LastName = request.LastName;
            patient.Dob = request.Dob;
            patient.Gender = request.Gender;
            patient.Phone = request.Phone;
            patient.Address = request.Address;
            patient.InsuranceNumber = request.InsuranceNumber;

            // Update User
            user.Email = request.Email;
            user.Username = request.Username;

            await _unitOfWork.PatientRepository.UpdateAsync(patient);
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }


    }
}
