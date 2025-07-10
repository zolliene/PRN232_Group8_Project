using Services.Dto.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
   public interface ILabTestService
    {
        Task<List<GetLabTestResq>> GetAllLabTest(DateTime date);
        Task<GetLabTestResq> GetLabTestById(int id);
        Task<CreateLabTestDto> CreateLabTestId(CreateLabTestDto input);
        Task<GetLabTestResq> UpdateLabTestId(int id, CreateLabTestDto input);
        Task DeleteLabTestId(int id);
    }
}
