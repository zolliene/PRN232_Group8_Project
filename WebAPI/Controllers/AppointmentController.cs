using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.response;
using Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AppointmentController : BaseController
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [Authorize(Roles = "receptionist, doctor")]
    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery] DateOnly? date, [FromQuery] string? status)
    {
        try
        {
            
            var response = await _appointmentService.GetAllAppointments(date, status);
            return Ok(ApiResponse<IList<GetAppointmentRes>>.OkResponse(response, "Appointment list"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(AppointmentController));
        }
    }
    
    [Authorize(Roles = "receptionist")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment([FromRoute] int id)
    {
        try
        {
            await _appointmentService.UpdateAppointment(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(AppointmentController));
        }
    }

    [Authorize(Roles = "doctor")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById([FromRoute] int id)
    {
        try
        {
            var response = await _appointmentService.GetAppointmentById(id);
            return Ok(ApiResponse<GetPatientDetail>.OkResponse(response, "Patient Detail"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(AppointmentController));
        }
    }
    
    
}