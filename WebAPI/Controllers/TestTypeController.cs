using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto.response;
using Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "doctor")]
public class TestTypeController : BaseController
{
    private readonly ITestTypeService _testTypeService;

    public TestTypeController(ITestTypeService testTypeService)
    {
        _testTypeService = testTypeService;
    }


    [HttpGet]
    public async Task<IActionResult> GetListTestType()
    {
        try
        {
            var response = await _testTypeService.GetListTestType();
            return Ok(ApiResponse<IList<GetTestTypeRes>>.OkResponse(response, "Get list test type successfully"));
        }
        catch (Exception e)
        {
            return HandleException(e, nameof(TestTypeController));
        }
    }
}