using Services.Dto;
using Services.Dto.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICrudDoctorService
    {
        Task<List<DoctorDTO>> GetCrudDoctorAccountsAsync();
        Task<DoctorDTO> GetCrudDoctorAccountByIdAsync(int id);
        Task<bool> CreateDoctorAsync(CreateDoctorDTO dto);
        Task<bool> UpdateDoctorAsync(int userId, UpdateDoctorDTO dto);
        Task<bool> DeleteDoctorAsync(int id);      
     //   Task<int?> CreateUserAsync(CreateUserDTO dto); 
        

    }
}
