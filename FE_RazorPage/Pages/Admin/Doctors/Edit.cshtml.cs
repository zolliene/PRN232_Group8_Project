using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Models;
using System.Text;
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
            if (!response.IsSuccessStatusCode) return RedirectToPage("Index");

            var json = await response.Content.ReadAsStringAsync();
            Doctor = JsonSerializer.Deserialize<UpdateDoctorModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var content = new StringContent(
                JsonSerializer.Serialize(Doctor),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"api/Doctorccount/{Doctor.Id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật bác sĩ.");
            return Page();
        }
    }

}
