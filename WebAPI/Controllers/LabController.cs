using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Dto.response;
using Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabController : BaseController
{
    private readonly ILabTestService _labTestService;

    public LabController(ILabTestService labTestService)
    {
        _labTestService = labTestService;
    }

    [Authorize(Roles = "doctor")]
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

    [Authorize(Roles = "doctor")]
    [HttpGet("results")]
    public async Task<IActionResult> GetLabResultsWithResults()
    {
        try
        {
            var response = await _labTestService.GetLabResultsWithResults(null);
            return Ok(ApiResponse<IList<GetLabResultRes>>.OkResponse(response, "Get lab results successfully"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(LabController));
        }
    }

    [Authorize(Roles = "doctor")]
    [HttpGet("results/{id}")]
    public async Task<IActionResult> GetLabResultById([FromRoute] int id)
    {
        try
        {
            var response = await _labTestService.GetLabResultById(id);
            return Ok(ApiResponse<GetLabResultRes>.OkResponse(response, "Get lab result detail successfully"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(LabController));
        }
    }
}