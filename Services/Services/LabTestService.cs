using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PlanyApp.Repository.Base;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto.request;
using Services.Interfaces;

namespace Services.Services;

public class LabTestService : ILabTestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<LabTestService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GenericRepository<LabTest> _labRepository;
    private readonly GenericRepository<TestType> _typeRepository;

    public LabTestService(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<LabTestService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _labRepository = _unitOfWork.LabTestRepository;
        _typeRepository = _unitOfWork.TestTypeRepository;
    }

    public async Task CreateLabTest(CreateLabTestReq request)
    {
        try
        {
            _logger.LogInformation("Start create lab test for lab test with request {}", request);
            await _unitOfWork.BeginTransactionAsync();
            var testType = await _typeRepository.GetByIdAsync(request.TestTypeId);
            var labTest = _mapper.Map<LabTest>(request);
            labTest.OrderStatus = "Pending";
            labTest.ReferenceRange = testType.ReferenceRange;
            labTest.Unit = testType.Unit;

            await _labRepository.AddAsync(labTest);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation("Save lab test successfully");
        }
        catch (Exception e)
        {
            _logger.LogError("Error at create lab test cause by {}", e.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}