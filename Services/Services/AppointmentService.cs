using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.UnitOfWork;
using Services.Dto.response;
using Services.Interfaces;

namespace Services.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppointmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<GetAppointmentRes>> GetAllAppointments(DateOnly? date, string? status)
    {
        
        try
        {
            _logger.LogInformation("Get all appointments with date {} and status {}", date, status);
            var appointmentDate = date ?? DateOnly.FromDateTime(DateTime.Now);
            var appointmentStatus = status ?? "BOOKED";
            var listAppointment = await _unitOfWork.AppointmentRepository.GetAllAsync(query => query
                .Include(a => a.Patient)
                .Where(a => a.Status.ToLower() == appointmentStatus.ToLower() && a.Date == appointmentDate));
            _logger.LogInformation("Start converting to response dto");
            return _mapper.Map<List<GetAppointmentRes>>(listAppointment);
        }
        catch (Exception e)
        {
            _logger.LogError("Error at handle get all appointment cause by {}", e.Message);
            throw;
        }
    }
}