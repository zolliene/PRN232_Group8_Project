using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Dto.response;
using Services.Interfaces;
using System.Security.Claims;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : BaseController
{
    private readonly IAppointmentService _appointmentService;

    public BookingController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentReq request, int userId)
    //{
    //    try
    //    {
    //        await _appointmentService.CreateAppointment(request, userId);
    //        return Ok(ApiResponse<string>.OkResponse(null, "Appointment created successfully"));
    //    }
    //    catch (Exception ex)
    //    {
    //        return HandleException(ex, nameof(BookingController));
    //    }
    //}
    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentReq request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException());
        await _appointmentService.CreateAppointment(request, userId);
        return Ok(ApiResponse<string>.OkResponse(null, "Appointment created successfully"));
    }

    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetPatientExaminationHistory([FromRoute] int userId)
    {
        try
        {
            var result = await _appointmentService.GetPatientExaminationHistory(userId);
            return Ok(ApiResponse<IList<GetPatientExaminationHistoryRes>>.OkResponse(result, "Patient examination history"));
        }
        catch (Exception ex)
        {
            return HandleException(ex, nameof(BookingController));
        }
    }

}
