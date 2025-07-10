using Microsoft.AspNetCore.Mvc;
using Services.Dto;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArvDrugsController : ControllerBase
    {
        private readonly IArvDrugService _service;

        public ArvDrugsController(IArvDrugService service)
        {
            _service = service;
        }

        // 🗂️ GET: api/arvdrugs/groups
        [HttpGet("groups")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _service.GetAllGroupsAsync();
            return Ok(groups);
        }

        // 📄 GET: api/arvdrugs/group/{groupId}
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetDrugsByGroup(int groupId)
        {
            var drugs = await _service.GetDrugsByGroupIdAsync(groupId);
            return Ok(drugs);
        }

        // 📄 GET: api/arvdrugs
        [HttpGet]
        public async Task<IActionResult> GetAllDrugs()
        {
            var drugs = await _service.GetAllDrugsAsync();
            return Ok(drugs);
        }

        // 📄 GET: api/arvdrugs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrugById(int id)
        {
            var drug = await _service.GetDrugByIdAsync(id);
            if (drug == null) return NotFound("Drug not found.");
            return Ok(drug);
        }

        // 🟢 POST: api/arvdrugs
        [HttpPost]
        public async Task<IActionResult> CreateDrug([FromBody] CreateArvDrugDTO dto)
        {
            var success = await _service.CreateDrugAsync(dto);
            if (!success) return BadRequest("Create failed.");
            return Ok("Drug created successfully.");
        }

        // ✏️ PUT: api/arvdrugs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrug(int id, [FromBody] UpdateArvDrugDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch.");
            var success = await _service.UpdateDrugAsync(dto);
            if (!success) return NotFound("Drug not found.");
            return Ok("Drug updated successfully.");
        }

        // ❌ DELETE: api/arvdrugs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrug(int id)
        {
            var success = await _service.DeleteDrugAsync(id);
            if (!success) return NotFound("Drug not found.");
            return Ok("Drug deleted successfully.");
        }
    }
}
