using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FE_RazorPage.Pages.Admin.Doctors
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("MyApi");
        }

        [BindProperty]
        public GetDoctorModel Doctor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoctorAccount/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var doctor = await response.Content.ReadFromJsonAsync<GetDoctorModel>();
            if (doctor == null) return NotFound();

            Doctor = doctor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.DeleteAsync($"api/DoctorAccount/{Doctor.Id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Xóa thất bại.");
            return Page();
        }
    }
}

