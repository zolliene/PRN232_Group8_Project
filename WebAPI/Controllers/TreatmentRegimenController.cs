using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Dto.response;
using Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "doctor")]
public class TreatmentRegimenController : BaseController
{
    private readonly ITreatmentRegimenService _treatmentRegimenService;

    public TreatmentRegimenController(ITreatmentRegimenService treatmentRegimenService)
    {
        _treatmentRegimenService = treatmentRegimenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTreatmentRegimens()
    {
        try
        {
            var response = await _treatmentRegimenService.GetAllTreatmentRegimens();
            return Ok(ApiResponse<IList<GetTreatmentRegimenRes>>.OkResponse(response, "Get treatment regimens successfully"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(TreatmentRegimenController));
        }
    }

    [HttpGet("patient/{appointmentId}")]
    public async Task<IActionResult> GetPatientByAppointmentId([FromRoute] int appointmentId)
    {
        try
        {
            var response = await _treatmentRegimenService.GetPatientByAppointmentId(appointmentId);
            return Ok(ApiResponse<GetPatientDetail>.OkResponse(response, "Get patient detail successfully"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(TreatmentRegimenController));
        }
    }

    [HttpPost("patient-regimen")]
    public async Task<IActionResult> CreatePatientRegimen([FromBody] CreatePatientRegimenReq request)
    {
        try
        {
            await _treatmentRegimenService.CreatePatientRegimen(request);
            return Ok(ApiResponse<string>.CreateResponse("Patient regimen created successfully"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(TreatmentRegimenController));
        }
    }
}