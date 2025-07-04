using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "doctor")]
public class LabController : BaseController
{
    private readonly ILabTestService _labTestService;

    public LabController(ILabTestService labTestService)
    {
        _labTestService = labTestService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateLabTest(CreateLabTestReq request)
    {
        try
        {
            await _labTestService.CreateLabTest(request);
            return Ok(ApiResponse<string>.CreateResponse("Created Lab Test successfully"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(LabController));
        }
    }
    
}