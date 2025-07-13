using Services.Dto.request;
using Services.Dto.response;

namespace Services.Interfaces
{
    public interface IPatientService
    {
        Task<PatientProfileRes> GetProfile(int userId);
        Task UpdateProfile(int userId, UpdatePatientProfileReq request);
    }
}
