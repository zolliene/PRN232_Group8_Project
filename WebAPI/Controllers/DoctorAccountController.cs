using Microsoft.AspNetCore.Mvc;
using Services.Dto;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorAccountController : ControllerBase
    {
        private readonly ICrudDoctorService _crudDoctorService;

        public DoctorAccountController(ICrudDoctorService crudDoctorService)
        {
            _crudDoctorService = crudDoctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _crudDoctorService.GetCrudDoctorAccountsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _crudDoctorService.GetCrudDoctorAccountByIdAsync(id);

            if (doctor == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"Không tìm thấy bác sĩ với ID = {id}",
                    IsSuccess = false
                });
            }

            return Ok(doctor);
        }


        //[HttpPost("create-user")]
        //public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        //{
        //    var userId = await _crudDoctorService.CreateUserAsync(dto);
        //    if (userId == null)
        //        return BadRequest("Username đã tồn tại");

        //    return Ok(new { message = "Tạo user thành công", userId });
        //}

        [HttpPost("create-doctor")]
        public async Task<IActionResult> Create([FromBody] CreateDoctorDTO dto)
        {
            var success = await _crudDoctorService.CreateDoctorAsync(dto);
            if (!success) return BadRequest("Failed to create doctor.");
            return Ok("Doctor created.");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(int userId, [FromBody] UpdateDoctorDTO dto)
        {
            var success = await _crudDoctorService.UpdateDoctorAsync(userId, dto);
            if (!success) return NotFound("Doctor not found.");
            return Ok("Doctor updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var success = await _crudDoctorService.DeleteDoctorAsync(id);
            if (!success)
                return NotFound("Doctor not found or already deleted.");
            return Ok("Doctor deleted successfully");

        }
    }
}
