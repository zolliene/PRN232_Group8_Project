using Services.Dto.response;

namespace Services.Interfaces;

public interface ITestTypeService
{
    Task<IList<GetTestTypeRes>> GetListTestType();
}