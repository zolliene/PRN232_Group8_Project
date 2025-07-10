using Services.Dto.request;
using Services.Dto.response;

namespace Services.Interfaces;

public interface ILabTestService
{
    Task CreateLabTest(CreateLabTestReq request);
    Task<IList<GetLabResultRes>> GetLabResultsWithResults(DateOnly? date);
    Task<GetLabResultRes> GetLabResultById(int id);
}