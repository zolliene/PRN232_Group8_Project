using Services.Dto.request;

namespace Services.Interfaces;

public interface ILabTestService
{
    Task CreateLabTest(CreateLabTestReq request);
}