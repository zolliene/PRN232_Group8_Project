using Microsoft.AspNetCore.Mvc;
using Services.Dto.request;
using Services.Dto.response;
using Services.Interfaces;
using Services.Services;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabTestV1Controller : Controller
    {
        private readonly ILabTestServiceV1 _IlasbTestService;
        public LabTestV1Controller(ILabTestServiceV1 labTestService)
        {
            _IlasbTestService = labTestService;
        }
        [HttpGet("getAllInADay")]
        public async Task<IActionResult> GetAllInday([FromQuery] DateTime date)
        {
            var res = await _IlasbTestService.GetAllLabTestByDate(date);
            return Ok(new ApiResponse<Object>
            {
                Data = res,
                Status = 200,
                Message = "Lấy danh sách xét nghiệm thành công"
            });
        }
        [HttpGet("getByAppointmentId")]
        public async Task<IActionResult> GetbyAppointmentId([FromQuery] int appointmentId)
        {
            var res = await _IlasbTestService.GetAllLabTestByAppointmentId(appointmentId);
            return Ok(new ApiResponse<Object>
            {
                Data = res,
                Status = 200,
                Message = "Lấy xét nghiệm theo id của lịch hẹn" + appointmentId + " thành công"
            });
        }
        [HttpGet("getById")]
        public async Task<IActionResult> GetbyId([FromQuery] int id)
        {
            var res = await _IlasbTestService.GetLabTestById(id);
            return Ok(new ApiResponse<Object>
            {
                Data = res,
                Status = 200,
                Message = "lấy xét nghiệm theo id" + id + "thành công"
            });
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllLabTest()
        {
            var res = await _IlasbTestService.GetAllLabtest();
            return Ok(new ApiResponse<Object>
            {
                Data = res,
                Status = 200,
                Message = "Lấy tất cả xét nghiệm thành công"
            });
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateLabTest([FromBody] CreateLabTestDtoV1 dto)
        {
            var res = await _IlasbTestService.CreateLabTestId(dto);
            return Ok(new ApiResponse<Object>
            {
                Data = res,
                Status = 200,
                Message = "Tạo xét nghiệm thành công"
            });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateLabTest([FromQuery] int id, [FromBody] UpdateLabTestDto dto)
        {
            Console.WriteLine($"🟡 Update ID: {id}");
            Console.WriteLine($"🟡 Value: {dto.ResultValue}, Status: {dto.ResultStatus}, Comments: {dto.Comments}");
            var res = await _IlasbTestService.UpdateLabTestId(id, dto);
            Console.WriteLine($"🟡 Update ID: {id}");
            Console.WriteLine($"🟡 Value: {dto.ResultValue}, Status: {dto.ResultStatus}, Comments: {dto.Comments}");
            return Ok(new ApiResponse<Object>
            {
                Data = res,
                Status = 200,
                Message = "Cập nhật xét nghiệm thành công"
            });
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteLabTest([FromQuery] int id)
        {
            await _IlasbTestService.DeleteLabTestId(id);
            return Ok(new ApiResponse<Object>
            {
                Data = null,
                Status = 200,
                Message = "Xóa xét nghiệm thành công"
            });
        }
    }
}
