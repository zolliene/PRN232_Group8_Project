using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "doctor")]
public class ExaminationController : BaseController
{
    private readonly IExaminationService _examinationService;

    public ExaminationController(IExaminationService examinationService)
    {
        _examinationService = examinationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddExamination([FromBody] CreateExaminationReq request)
    {
        try
        {
            await _examinationService.AddExamination(request);
            return Ok(ApiResponse<string>.CreateResponse("Examination successfully added."));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(ExaminationController));
        }
    } 
    
    
}