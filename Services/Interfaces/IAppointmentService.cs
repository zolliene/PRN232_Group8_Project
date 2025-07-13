using Repositories.Models;
using Services.Dto.request;
using Services.Dto.response;

namespace Services.Interfaces;

public interface IAppointmentService
{
     Task<List<GetAppointmentRes>> GetAllAppointments(DateOnly? date, string? status);

     Task UpdateAppointment(int appointmentId);

     Task<GetPatientDetail> GetAppointmentById(int appointmentId);
    Task CreateAppointment(CreateAppointmentReq request,int userId);
    Task<List<GetPatientExaminationHistoryRes>> GetPatientExaminationHistory(int patientId);


}