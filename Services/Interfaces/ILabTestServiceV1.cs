using Services.Dto.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
   public interface ILabTestServiceV1
    {
        Task<List<GetLabTestResqV1>> GetAllLabTest(DateTime date);
        Task<GetLabTestResqV1> GetLabTestById(int id);
        Task<CreateLabTestDtoV1> CreateLabTestId(CreateLabTestDtoV1 input);
        Task<GetLabTestResqV1> UpdateLabTestId(int id, CreateLabTestDtoV1 input);
        Task DeleteLabTestId(int id);
    }
}
