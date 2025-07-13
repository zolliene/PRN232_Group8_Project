using Services.Dto.request;
using Services.Dto.response;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITreatmentRegimenService
    {
        // Phác đồ chính
        Task<List<TreatmentRegimenDTO>> GetAllRegimensAsync();
        Task<TreatmentRegimenDTO?> GetRegimenByIdAsync(int id);
        Task<bool> CreateRegimenAsync(CreateTreatmentRegimenDTO dto);
        Task<bool> UpdateRegimenAsync(UpdateTreatmentRegimenDTO dto);
        Task<bool> DeleteRegimenAsync(int id);

        // Thuốc trong phác đồ
        Task<List<RegimenDrugDetailDTO>> GetDrugsByRegimenIdAsync(int regimenId);
        Task<bool> AddDrugToRegimenAsync(AddRegimenDrugDTO dto);
        Task<bool> UpdateRegimenDrugAsync(UpdateRegimenDrugDTO dto);
        Task<bool> DeleteRegimenDrugAsync(int regimenDrugId);


        Task<IList<GetTreatmentRegimenRes>> GetAllTreatmentRegimens();
        Task CreatePatientRegimen(CreatePatientRegimenReq request);
        Task<GetPatientDetail> GetPatientByAppointmentId(int appointmentId);
    }
}