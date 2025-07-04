using Services.Dto.response;

namespace Services.Interfaces;

public interface IAppointmentService
{
     Task<List<GetAppointmentRes>> GetAllAppointments(DateOnly? date, string? status);

     Task UpdateAppointment(int appointmentId);

}