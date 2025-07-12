using System.Security.Claims;
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

public class TreatmentRegimenService : ITreatmentRegimenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TreatmentRegimenService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GenericRepository<TreatmentRegimensMaster> _treatmentRegimenRepository;
    private readonly GenericRepository<PatientRegimen> _patientRegimenRepository;
    private readonly GenericRepository<Examination> _examinationRepository;
    private readonly GenericRepository<Appointment> _appointmentRepository;

    public TreatmentRegimenService(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<TreatmentRegimenService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _treatmentRegimenRepository = _unitOfWork.TreatmentRegimensMasterRepository;
        _patientRegimenRepository = _unitOfWork.PatientRegimenRepository;
        _examinationRepository = _unitOfWork.ExaminationRepository;
        _appointmentRepository = _unitOfWork.AppointmentRepository;
    }

    public async Task<IList<GetTreatmentRegimenRes>> GetAllTreatmentRegimens()
    {
        try
        {
            _logger.LogInformation("Start getting all treatment regimens");
            
            var regimens = await _treatmentRegimenRepository.GetAllAsync(query => query
                .Include(tr => tr.RegimenDrugs)
                    .ThenInclude(rd => rd.Drug)
                        .ThenInclude(d => d.Group)
            );

            return _mapper.Map<IList<GetTreatmentRegimenRes>>(regimens);
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get all treatment regimens cause by {}", e.Message);
            throw;
        }
    }

    public async Task CreatePatientRegimen(CreatePatientRegimenReq request)
    {
        try
        {
            _logger.LogInformation("Start creating patient regimen for appointment {}", request.AppointmentId);
            await _unitOfWork.BeginTransactionAsync();

            // Tìm examination từ appointment
            var examination = await _examinationRepository.FirstOrDefaultAsync(e => e.AppointmentId == request.AppointmentId);
            if (examination == null)
                throw new KeyNotFoundException("Examination not found for this appointment");

            // Kiểm tra xem đã có patient regimen chưa
            var existingRegimen = await _patientRegimenRepository.FirstOrDefaultAsync(pr => pr.ExaminationId == examination.Id);
            if (existingRegimen != null)
            {
                // Nếu đã tồn tại, cập nhật thay vì tạo mới
                existingRegimen.RegimenMasterId = request.RegimenMasterId;
                existingRegimen.CustomNotes = request.CustomNotes;
                await _patientRegimenRepository.UpdateAsync(existingRegimen);
                _logger.LogInformation("Updated existing patient regimen");
            }
            else
            {
                // Tạo mới nếu chưa tồn tại
                var patientRegimen = _mapper.Map<PatientRegimen>(request);
                patientRegimen.ExaminationId = examination.Id;
                await _patientRegimenRepository.AddAsync(patientRegimen);
                _logger.LogInformation("Created new patient regimen");
            }

            // Cập nhật status appointment thành success
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = "success";
                await _appointmentRepository.UpdateAsync(appointment);
            }

            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();
            
            _logger.LogInformation("Patient regimen processed successfully");
        }
        catch (Exception e)
        {
            _logger.LogError("Error at create patient regimen cause by {}", e.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<GetPatientDetail> GetPatientByAppointmentId(int appointmentId)
    {
        try
        {
            _logger.LogInformation("Start get patient by appointment id {}", appointmentId);
            
            var appointment = (await _appointmentRepository.FindIncludeAsync(
                a => a.Id == appointmentId,
                a => a.Patient
            )).FirstOrDefault();

            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            var response = _mapper.Map<GetPatientDetail>(appointment.Patient);
            response.AppointmentId = appointmentId;
            
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get patient by appointment id cause by {}", e.Message);
            throw;
        }
    }
}