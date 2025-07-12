using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Repositories.Models;
using Services.Dto;
using System.Text.Json;

namespace FE_RazorPage.Pages.Admin.Doctors
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IHttpClientFactory factory, ILogger<EditModel> logger)
        {
            _httpClient = factory.CreateClient("MyApi");
            _logger = logger;
        }

        [BindProperty]
        public UpdateDoctorModel Doctor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _logger.LogInformation("Bắt đầu OnGetAsync với id = {Id}", id);

            var response = await _httpClient.GetAsync($"api/DoctorAccount/{id}");
            _logger.LogInformation("Gọi API GET với status code: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var doctorData = await response.Content.ReadFromJsonAsync<UpdateDoctorModel>();
            if (doctorData == null)
                return NotFound();

            _logger.LogInformation("Dữ liệu bác sĩ nhận được: UserId={UserId}, Name={Name}", doctorData.Id, doctorData.Name);

            Doctor = new UpdateDoctorModel
            {
                Id = doctorData.Id,
                Name = doctorData.Name,
                Email = doctorData.Email,
                LicenseNumber = doctorData.LicenseNumber,
                Qualification = doctorData.Qualification,
                DateOfLicense = doctorData.DateOfLicense,
                LicenseExpiryDate = doctorData.LicenseExpiryDate
            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Bắt đầu OnPostAsync với dữ liệu: UserId={UserId}, Name={Name}", Doctor.Id, Doctor.Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState không hợp lệ.");
                return Page();
            }

            var response = await _httpClient.PutAsJsonAsync($"api/DoctorAccount/{Doctor.Id}", Doctor);
            _logger.LogInformation("Gọi API PUT tới /api/DoctorAccount/{UserId} trả về status code: {StatusCode}", Doctor.Id, response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Cập nhật thành công, chuyển hướng về Index.");
                return RedirectToPage("Index");
            }

            _logger.LogError("Cập nhật thất bại với mã lỗi: {StatusCode}", response.StatusCode);
            ModelState.AddModelError(string.Empty, "Cập nhật thất bại.");
            return Page();
        }


    }
}
    