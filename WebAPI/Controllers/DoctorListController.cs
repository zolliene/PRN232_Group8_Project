using Microsoft.AspNetCore.Mvc;
using Services.Dto.response;
using Services.Interfaces;
using WebAPI.Controllers;
using WebAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class DoctorListController : BaseController
{
    private readonly IDoctorListService _doctorListService;

    public DoctorListController(IDoctorListService doctorListService)
    {
        _doctorListService = doctorListService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors()
    {
        try
        {
            var result = await _doctorListService.GetAllDoctors();
            return Ok(ApiResponse<IList<GetDoctorRes>>.OkResponse(result, "Doctor list"));
        }
        catch (Exception ex)
        {
            return HandleException(ex, nameof(DoctorListController));
        }
    }
}
