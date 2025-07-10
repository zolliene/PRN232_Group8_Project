using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : BaseController
{
    private readonly IAppointmentService _appointmentService;

    public BookingController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentReq request)
    {
        try
        {
            await _appointmentService.CreateAppointment(request);
            return Ok(ApiResponse<string>.OkResponse(null, "Appointment created successfully"));
        }
        catch (Exception ex)
        {
            return HandleException(ex, nameof(BookingController));
        }
    }
}
