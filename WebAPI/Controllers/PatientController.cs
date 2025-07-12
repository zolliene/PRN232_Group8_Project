using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Interfaces;
using WebAPI.Models;
using System.Security.Claims;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "patient")]
public class PatientController : BaseController
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _patientService.GetProfile(userId);
            return Ok(ApiResponse<object>.OkResponse(result, "Patient profile"));
        }
        catch (Exception ex)
        {
            return HandleException(ex, nameof(PatientController));
        }
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdatePatientProfileReq request)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _patientService.UpdateProfile(userId, request);
            return Ok(ApiResponse<string>.OkResponse(null, "Profile updated successfully"));
        }
        catch (Exception ex)
        {
            return HandleException(ex, nameof(PatientController));
        }
    }
}
