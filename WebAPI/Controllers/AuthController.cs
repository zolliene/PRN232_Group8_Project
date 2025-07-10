using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Services.Dto.request; 
using Services.Interfaces;
using WebAPI.Models;
using WebAPI.Controllers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(Services.Dto.LoginRequest request)
        {
            var result = _authService.Authenticate(request.Email, request.Password);
            if (result == null)
                return Unauthorized(new ApiResponse<object>
                {
                    Data = null,
                    Status = 401,
                    Message = "Sai tài khoản hoặc mật khẩu!"
                });

            return Ok(new ApiResponse<object>
            {
                Data = new { id = result.Id, token = result.Token, role = result.Role },
                Status = 200,
                Message = "Đăng nhập thành công"
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterPatientReq request)
        {
            try
            {
                await _authService.RegisterPatient(request);
                return Ok(ApiResponse<string>.OkResponse("Success", "Registered successfully"));
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(AuthController));
            }
        }

    }
}