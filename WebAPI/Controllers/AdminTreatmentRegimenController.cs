using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dto;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminTreatmentRegimenController : ControllerBase
    {
        private readonly ITreatmentRegimenService _service;

        public AdminTreatmentRegimenController(ITreatmentRegimenService service)
        {
            _service = service;
        }

        // 📄 GET: api/treatmentregimen
        [HttpGet]
        public async Task<IActionResult> GetAllRegimens()
        {
            var data = await _service.GetAllRegimensAsync();
            return Ok(data);
        }

        // 📄 GET: api/treatmentregimen/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegimenById(int id)
        {
            var result = await _service.GetRegimenByIdAsync(id);
            if (result == null) return NotFound("Regimen not found");
            return Ok(result);
        }

        // 🟢 POST: api/treatmentregimen
        [HttpPost]
        public async Task<IActionResult> CreateRegimen([FromBody] CreateTreatmentRegimenDTO dto)
        {
            var success = await _service.CreateRegimenAsync(dto);
            if (!success) return BadRequest("Creation failed");
            return Ok("Regimen created successfully");
        }

        // ✏️ PUT: api/treatmentregimen/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegimen(int id, [FromBody] UpdateTreatmentRegimenDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var success = await _service.UpdateRegimenAsync(dto);
            if (!success) return NotFound("Regimen not found");
            return Ok("Regimen updated successfully");
        }

        // ❌ DELETE: api/treatmentregimen/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegimen(int id)
        {
            var success = await _service.DeleteRegimenAsync(id);
            if (!success) return NotFound("Regimen not found");
            return Ok("Regimen deleted successfully");
        }

        // 📋 GET: api/treatmentregimen/{regimenId}/drugs
        [HttpGet("{regimenId}/drugs")]
        public async Task<IActionResult> GetDrugsInRegimen(int regimenId)
        {
            var data = await _service.GetDrugsByRegimenIdAsync(regimenId);
            return Ok(data);
        }

        // ➕ POST: api/treatmentregimen/{regimenId}/drugs
        [HttpPost("{regimenId}/drugs")]
        public async Task<IActionResult> AddDrugToRegimen(int regimenId, [FromBody] AddRegimenDrugDTO dto)
        {
            if (dto.RegimenId != regimenId) return BadRequest("ID mismatch");
            var success = await _service.AddDrugToRegimenAsync(dto);
            if (!success) return BadRequest("Failed to add drug");
            return Ok("Drug added successfully");
        }

        // ✏️ PUT: api/treatmentregimen/drugs/{drugId}
        [HttpPut("drugs/{drugId}")]
        public async Task<IActionResult> UpdateRegimenDrug(int drugId, [FromBody] UpdateRegimenDrugDTO dto)
        {
            if (dto.Id != drugId) return BadRequest("ID mismatch");
            var success = await _service.UpdateRegimenDrugAsync(dto);
            if (!success) return NotFound("Regimen drug not found");
            return Ok("Drug updated successfully");
        }

        // ❌ DELETE: api/treatmentregimen/drugs/{drugId}
        [HttpDelete("drugs/{drugId}")]
        public async Task<IActionResult> DeleteRegimenDrug(int drugId)
        {
            var success = await _service.DeleteRegimenDrugAsync(drugId);
            if (!success) return NotFound("Regimen drug not found");
            return Ok("Drug removed from regimen");
        }
    }

}
