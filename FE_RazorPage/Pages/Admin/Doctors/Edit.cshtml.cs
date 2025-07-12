using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto;
using System.Text.Json;

namespace FE_RazorPage.Pages.Admin.Doctors
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("MyApi");
        }

        [BindProperty]
        public UpdateDoctorModel Doctor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoctorAccount/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var doctorData = await response.Content.ReadFromJsonAsync<GetDoctorModel>();
            if (doctorData == null) return NotFound();

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
            if (!ModelState.IsValid)
                return Page();
            var response = await _httpClient.PutAsJsonAsync($"api/DoctorAccount/{Doctor.Id}", Doctor);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Cập nhật thất bại.");
            return Page();
        }
    }
}
    