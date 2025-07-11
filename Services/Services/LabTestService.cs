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
    public async Task<IList<GetAppointmentWithLabRes>> GetAppointmentsWithLabResults()
    {
        try
        {
            _logger.LogInformation("Start getting appointments with lab results for recent 3 days");
            
            // Lấy appointments có lab results trong 3 ngày gần đây
            var endDate = DateOnly.FromDateTime(DateTime.Now);
            var startDate = endDate.AddDays(-3);
            
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync(query => query
                .Include(a => a.Patient)
                .Include(a => a.LabTests.Where(lt => !string.IsNullOrEmpty(lt.ResultValue)))
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .Where(a => a.LabTests.Any(lt => !string.IsNullOrEmpty(lt.ResultValue))) // Chỉ lấy appointments có lab results
                .OrderByDescending(a => a.Date)
            );

            var result = appointments.Select(a => new GetAppointmentWithLabRes
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                PatientPhone = a.Patient.Phone,
                PatientDob = a.Patient.Dob,
                PatientGender = a.Patient.Gender,
                AppointmentDate = a.Date ?? DateOnly.FromDateTime(DateTime.Now),
                Session = a.Session,
                Status = a.Status,
                LabResultCount = a.LabTests.Count(lt => !string.IsNullOrEmpty(lt.ResultValue))
            }).ToList();

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get appointments with lab results cause by {}", e.Message);
            throw;
        }
    }
    public async Task<GetAppointmentLabResultsRes> GetAppointmentLabResults(int appointmentId)
{
    try
    {
        _logger.LogInformation("Start getting appointment lab results for appointment {}", appointmentId);
        
        var appointment = (await _unitOfWork.AppointmentRepository.FindIncludeAsync(
            a => a.Id == appointmentId,
            a => a.Patient
        )).FirstOrDefault();

        if (appointment == null)
            throw new KeyNotFoundException("Appointment not found");

        // Get lab tests with includes for TestType and LabStaff
        var labTests = await _unitOfWork.LabTestRepository.GetAllAsync(query => query
            .Include(lt => lt.TestType)
            .Include(lt => lt.LabStaff)
            .Where(lt => lt.AppointmentId == appointmentId && !string.IsNullOrEmpty(lt.ResultValue))
            .OrderByDescending(lt => lt.ResultDate)
        );

        var result = new GetAppointmentLabResultsRes
        {
            AppointmentId = appointment.Id,
            PatientName = appointment.Patient.FirstName + " " + appointment.Patient.LastName,
            PatientPhone = appointment.Patient.Phone,
            PatientDob = appointment.Patient.Dob,
            PatientGender = appointment.Patient.Gender,
            PatientAddress = appointment.Patient.Address,
            PatientInsuranceNumber = appointment.Patient.InsuranceNumber,
            AppointmentDate = appointment.Date ?? DateOnly.FromDateTime(DateTime.Now),
            Session = appointment.Session,
            LabResults = labTests.Select(lt => new LabResultDetailRes
            {
                Id = lt.Id,
                TestTypeName = lt.TestType.Name,
                TestTypeCode = lt.TestType.Code,
                OrderTime = lt.OrderTime,
                ResultValue = lt.ResultValue,
                Unit = lt.Unit,
                ReferenceRange = lt.ReferenceRange,
                ResultStatus = lt.ResultStatus,
                Comments = lt.Comments,
                ResultDate = lt.ResultDate,
                LabStaffName = lt.LabStaff?.Username ?? "Lab Staff"
            }).ToList()
        };

        return result;
    }
    catch (Exception e)
    {
        _logger.LogError("Error at get appointment lab results cause by {}", e.Message);
        throw;
    }
}
}