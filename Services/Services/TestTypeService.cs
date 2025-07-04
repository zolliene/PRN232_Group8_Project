using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlanyApp.Repository.Base;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto.response;
using Services.Interfaces;

namespace Services.Services;

public class TestTypeService : ITestTypeService
{
    private readonly ILogger<TestTypeService> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GenericRepository<TestType> _testTypeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TestTypeService(ILogger<TestTypeService> logger, IMapper mapper, 
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _testTypeRepository = _unitOfWork.TestTypeRepository;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<IList<GetTestTypeRes>> GetListTestType()
    {
        try
        {
            _logger.LogInformation("Start getting list test type");
            var testTypeList = await _testTypeRepository.FindAsync(t => t.IsActive == true);
            if (testTypeList.Count == 0) throw new NullReferenceException("Test type not found");
            
            return _mapper.Map<IList<GetTestTypeRes>>(testTypeList);
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get list test type cause by {}", e.Message);
            throw;
        }
    }
    
    
    
}