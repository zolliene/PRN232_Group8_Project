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
using Services.Dto;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // 🗂️ Lấy danh sách tất cả phác đồ
        public async Task<List<TreatmentRegimenDTO>> GetAllRegimensAsync()
    {
        var regimens = await _unitOfWork.TreatmentRegimensMasterRepository.GetAllAsync();
        return regimens.Select(r => new TreatmentRegimenDTO
        {
            Id = r.Id,
            RegimenName = r.RegimenName,
            Drugs = r.Drugs,
            Description = r.Description
        }).ToList();
    }
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

            // Xóa phác đồ
            _unitOfWork.TreatmentRegimensMasterRepository.Remove(regimen);
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

         // 📄 Lấy chi tiết 1 phác đồ
        public async Task<TreatmentRegimenDTO?> GetRegimenByIdAsync(int id)
{
    var regimen = await _unitOfWork.TreatmentRegimensMasterRepository.GetByIdAsync(id);
    if (regimen == null) return null;

    return new TreatmentRegimenDTO
    {
        Id = regimen.Id,
        RegimenName = regimen.RegimenName,
        Drugs = regimen.Drugs,
        Description = regimen.Description
    };
}

// 🟢 Tạo mới phác đồ
public async Task<bool> CreateRegimenAsync(CreateTreatmentRegimenDTO dto)
{
    var entity = _mapper.Map<TreatmentRegimensMaster>(dto);
    await _unitOfWork.TreatmentRegimensMasterRepository.AddAsync(entity);
    await _unitOfWork.SaveAsync();
    return true;
}

// ✏️ Cập nhật phác đồ
public async Task<bool> UpdateRegimenAsync(UpdateTreatmentRegimenDTO dto)
{
    var regimen = await _unitOfWork.TreatmentRegimensMasterRepository.GetByIdAsync(dto.Id);
    if (regimen == null) return false;

    regimen.RegimenName = dto.RegimenName;
    regimen.Drugs = dto.Drugs;
    regimen.Description = dto.Description;

    await _unitOfWork.SaveAsync();
    return true;
}

// ❌ Xóa phác đồ
public async Task<bool> DeleteRegimenAsync(int id)
{
    // Lấy phác đồ kèm theo các RegimenDrugs
    var regimenList = await _unitOfWork.TreatmentRegimensMasterRepository.GetAllAsync(q =>
        q.Include(r => r.RegimenDrugs).Where(r => r.Id == id));

    var regimen = regimenList.FirstOrDefault();
    if (regimen == null) return false;

    // Xóa các liên kết trung gian
    if (regimen.RegimenDrugs.Any())
    {
        _unitOfWork.RegimenDrugRepository.RemoveRange(regimen.RegimenDrugs);
    }

    // Xóa phác đồ
    _unitOfWork.TreatmentRegimensMasterRepository.Remove(regimen);
    await _unitOfWork.SaveAsync();
    return true;
}


// 📋 Lấy danh sách thuốc trong phác đồ
public async Task<List<RegimenDrugDetailDTO>> GetDrugsByRegimenIdAsync(int regimenId)
{
    var drugs = await _unitOfWork.RegimenDrugRepository.FindIncludeAsync(
        d => d.RegimenId == regimenId,
        d => d.Drug
    );

    return drugs.Select(d => new RegimenDrugDetailDTO
    {
        Id = d.Id,
        DrugId = d.DrugId,
        DrugName = d.Drug?.Name ?? "",
        Dose = d.Dose,
        Frequency = d.Frequency
    }).ToList();
}

// ➕ Thêm thuốc vào phác đồ
public async Task<bool> AddDrugToRegimenAsync(AddRegimenDrugDTO dto)
{
    var entity = new RegimenDrug
    {
        RegimenId = dto.RegimenId,
        DrugId = dto.DrugId,
        Dose = dto.Dose,
        Frequency = dto.Frequency
    };

    await _unitOfWork.RegimenDrugRepository.AddAsync(entity);
    await _unitOfWork.SaveAsync();
    return true;
}

// ✏️ Cập nhật thuốc trong phác đồ
public async Task<bool> UpdateRegimenDrugAsync(UpdateRegimenDrugDTO dto)
{
    var drug = await _unitOfWork.RegimenDrugRepository.GetByIdAsync(dto.Id);
    if (drug == null) return false;

    drug.Dose = dto.Dose;
    drug.Frequency = dto.Frequency;

    await _unitOfWork.SaveAsync();
    return true;
}

// ❌ Xóa thuốc khỏi phác đồ
public async Task<bool> DeleteRegimenDrugAsync(int regimenDrugId)
{
    var drug = await _unitOfWork.RegimenDrugRepository.GetByIdAsync(regimenDrugId);
    if (drug == null) return false;

    _unitOfWork.RegimenDrugRepository.Remove(drug);
    await _unitOfWork.SaveAsync();
    return true;
}


    }

}