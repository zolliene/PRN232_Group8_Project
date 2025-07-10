using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlanyApp.Repository.Base;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto.request;
using Services.Dto.response;
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

    public async Task<IList<GetLabResultRes>> GetLabResultsWithResults(DateOnly? date)
    {
        try
        {
            _logger.LogInformation("Start getting lab results with results for recent 3 days");
            
            // Lấy kết quả xét nghiệm của 3 ngày trước đến nay
            var endDate = DateOnly.FromDateTime(DateTime.Now);
            var startDate = endDate.AddDays(-3);
            
            var labTests = await _labRepository.GetAllAsync(query => query
                .Include(lt => lt.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(lt => lt.TestType)
                .Where(lt => !string.IsNullOrEmpty(lt.ResultValue)) // Chỉ lấy những test đã có kết quả
                .Where(lt => DateOnly.FromDateTime(lt.OrderTime) >= startDate && 
                           DateOnly.FromDateTime(lt.OrderTime) <= endDate) // Lọc 3 ngày trước đến nay
                .OrderByDescending(lt => lt.ResultDate)
            );

            return _mapper.Map<IList<GetLabResultRes>>(labTests);
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get lab results with results cause by {}", e.Message);
            throw;
        }
    }

    public async Task<GetLabResultRes> GetLabResultById(int id)
    {
        try
        {
            _logger.LogInformation("Start getting lab result by id: {}", id);
            
            var labTest = (await _labRepository.FindIncludeAsync(
                lt => lt.Id == id,
                lt => lt.Appointment.Patient,
                lt => lt.TestType
            )).FirstOrDefault();

            if (labTest == null)
                throw new KeyNotFoundException("Lab result not found");

            return _mapper.Map<GetLabResultRes>(labTest);
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get lab result by id cause by {}", e.Message);
            throw;
        }
    }
}