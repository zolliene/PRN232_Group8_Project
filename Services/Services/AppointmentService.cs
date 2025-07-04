using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlanyApp.Repository.Base;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto.response;
using Services.Interfaces;
namespace Services.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GenericRepository<Appointment> _appointmentRepository;
    
    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, 
        ILogger<AppointmentService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _appointmentRepository = _unitOfWork.AppointmentRepository;
    }

    public async Task<List<GetAppointmentRes>> GetAllAppointments(DateOnly? date, string? status)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            int? doctorId = null;
            string? finalStatus = null;

            if (role == "doctor")
            {
                if (!int.TryParse(userId, out var parsedDoctorId))
                    throw new UnauthorizedAccessException("Invalid doctor ID in token.");

                doctorId = parsedDoctorId;
                finalStatus = "CHECKED_IN"; 
            }
            else
            {
                finalStatus = string.IsNullOrWhiteSpace(status) ? null : status.ToLower();
            }

            _logger.LogInformation("Get all appointments with date {} and status {}", date, status);
            var appointmentDate = date ?? DateOnly.FromDateTime(DateTime.Now);
            var listAppointment = await _appointmentRepository.GetAllAsync(query => query
                .Include(a => a.Patient)
                .Where(a => a.Date == appointmentDate)
                .Where(a => string.IsNullOrWhiteSpace(finalStatus) || a.Status.ToLower() == finalStatus.ToLower())
                .Where(a => doctorId == null || a.DoctorId == doctorId)
            );
            _logger.LogInformation("Start converting to response dto");
            return _mapper.Map<List<GetAppointmentRes>>(listAppointment);
        }
        catch (Exception e)
        {
            _logger.LogError("Error at handle get all appointment cause by {}", e.Message);
            throw;
        }
    }

    public async Task UpdateAppointment(int appointmentId)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            _logger.LogInformation("Update an appointment with appointmentId {}", appointmentId);
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null) throw new KeyNotFoundException("Appointment not found");

            appointment.Status = "checked_in";
            appointment.CheckInTime = DateTime.Now;
            await _appointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError("Error at handle update appointment cause by {}", e.Message);
            throw;
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    public async Task<GetPatientDetail> GetAppointmentById(int appointmentId)
    {
        try
        {
            _logger.LogInformation("Start get appointment with appointmentId {}", appointmentId);
            var appointment = (await _appointmentRepository.FindIncludeAsync(
                a => a.Id == appointmentId,
                a => a.Patient
            )).FirstOrDefault() ?? throw new KeyNotFoundException("Appointment not found");

            return _mapper.Map<GetPatientDetail>(appointment.Patient);

        }
        catch (Exception e)
        {
            _logger.LogError("Error at get appointment by id cause by {}", e.Message);
            throw;
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}