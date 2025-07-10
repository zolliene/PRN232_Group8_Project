using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using FE_RazorPage.Models;

namespace FE_RazorPage.Pages.Admin.Doctors
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("MyApi");
        }

        [BindProperty]
        public CreateDoctorModel Doctor { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var json = JsonSerializer.Serialize(Doctor, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/doctoraccount/create-doctor", content);


            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            // THÊM ĐOẠN NÀY để hiển thị lỗi nếu có
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Lỗi khi tạo bác sĩ: " + error);
            return Page();
        }

    }

}
