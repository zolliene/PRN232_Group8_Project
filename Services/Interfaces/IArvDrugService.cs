using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IArvDrugService
    {
        Task<List<ArvDrugDTO>> GetAllDrugsAsync();
        Task<List<ArvDrugDTO>> GetDrugsByGroupIdAsync(int groupId);
        Task<ArvDrugDTO?> GetDrugByIdAsync(int id);
        Task<bool> CreateDrugAsync(CreateArvDrugDTO dto);
        Task<bool> UpdateDrugAsync(UpdateArvDrugDTO dto);
        Task<bool> DeleteDrugAsync(int id);

        Task<List<ArvDrugGroupDTO>> GetAllGroupsAsync();
    }

}
