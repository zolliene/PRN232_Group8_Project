using Microsoft.AspNetCore.Mvc;
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
            var res = await _IlasbTestService.GetAllLabTest(date);
            return Ok(new ApiResponse<Object>
            {
                Data = res,
                Status = 200,
                Message = "Lấy danh sách xét nghiệm thành công"
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
        public async Task<IActionResult> UpdateLabTest([FromQuery] int id, [FromBody] CreateLabTestDtoV1 dto)
        {
            var res = await _IlasbTestService.UpdateLabTestId(id, dto);
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
