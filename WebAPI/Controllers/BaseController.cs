using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleException(Exception ex, string controllerName)
    {
        switch (ex)
        {
            case KeyNotFoundException _:
                return NotFound(ApiResponse<string>.NotFoundResponse($"{ex.Message}"));

            case UnauthorizedAccessException _:
                return Unauthorized(ApiResponse<string>.UnauthorizedResponse($"{ex.Message}"));

            case ArgumentException _:
                return BadRequest(ApiResponse<string>.BadRequestResponse($"{ex.Message}"));

            case NullReferenceException _:
                return NotFound(ApiResponse<string>.NotFoundResponse($"{ex.Message}"));
            
            case ApplicationException _:
                return BadRequest(ApiResponse<string>.BadRequestResponse($"{ex.Message}"));
            
            case InvalidOperationException _:
                return StatusCode(500, ApiResponse<string>.InternalErrorResponse($"{ex.Message}"));

            default:
                return StatusCode(500, ApiResponse<string>.InternalErrorResponse($"{ex.Message}"));
        }
    }
}