using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PlanyApp.Repository.Base;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto.request;
using Services.Interfaces;

namespace Services.Services;

public class ExaminationService : IExaminationService
{
    private readonly ILogger<ExaminationService> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GenericRepository<Examination> _examinationRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExaminationService(ILogger<ExaminationService> logger, IMapper mapper, 
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _examinationRepository = _unitOfWork.ExaminationRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddExamination(CreateExaminationReq request)
    {
        try
        {
            _logger.LogInformation("Start handle add examination with request {}", request);
            await _unitOfWork.BeginTransactionAsync();
            var examination = _mapper.Map<Examination>(request);
            var doctorId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            examination.DoctorId = int.Parse(doctorId);
            examination.CreatedAt = DateTime.Now;
            
            await _examinationRepository.AddAsync(examination);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();
            
            _logger.LogInformation("Save examination successfully");
        }
        catch (Exception e)
        {
            _logger.LogError("Error at add examination cause by {}", e.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
       
    }
    
    
}