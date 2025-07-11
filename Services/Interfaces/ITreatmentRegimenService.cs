using Services.Dto.request;
using Services.Dto.response;

namespace Services.Interfaces;

public interface ITreatmentRegimenService
{
    Task<IList<GetTreatmentRegimenRes>> GetAllTreatmentRegimens();
    Task CreatePatientRegimen(CreatePatientRegimenReq request);
    Task<GetPatientDetail> GetPatientByAppointmentId(int appointmentId);
}